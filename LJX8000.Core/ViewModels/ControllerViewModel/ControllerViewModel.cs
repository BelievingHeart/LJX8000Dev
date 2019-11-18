using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Runtime.InteropServices;
using System.Windows.Input;
using System.Xml.Serialization;
using HalconDotNet;
using LJX8000.Core.Commands;
using LJX8000.Core.Enums;
using LJX8000.Core.Helpers;
using LJX8000.Core.ViewModels.Base;
using LJX8000.Core.ViewModels.ImageInfo;
using LJXNative;
using LJXNative.Data;
using PropertyChanged;

namespace LJX8000.Core.ViewModels.ControllerViewModel
{
    public sealed class ControllerViewModel : AutoSerializableBase<ControllerViewModel>
    {
 

        private void OnImageReady(HImage heightImage, HImage intensityImage)
        {
            ImageReady?.Invoke(heightImage, intensityImage);
        }

        #region Field

        /// <summary>Connection state</summary>
        private DeviceStatus _status = DeviceStatus.NoConnection;

        /// <summary>
        /// Set as field so that it would not be garbage collected
        /// </summary>
        private HighSpeedDataCallBackForSimpleArray _callbackSimpleArray;

        private int _rowsPerImage = 800;
        private bool _isConnectedHighSpeed = false;

        #endregion

        #region Property

        public override string Name    
        {
            get { return IpConfig.ToString(); }
            set { IpConfig = value; }
        }
        
        [DoNotNotify]
        [XmlIgnore]
        private IpConfigViewModel.IpConfigViewModel IpConfig
        {
            get { return _ipConfig; }
            set
            {
                _ipConfig = value;
                _deviceId = _ipConfig.ForthByte;
            }
        }

        /// <summary>
        /// Whether the received image should be displayed
        /// </summary>
        public bool ShouldImageBeDisplayed { get; set; } = true;

        [DoNotNotify]
        [XmlIgnore]
        public ICommand ConnectCommand { get; set; }

        [DoNotNotify]
        [XmlIgnore]
        public ICommand DisconnectCommand { get; set; }

        [DoNotNotify]
        [XmlIgnore]
        public ICommand InitHighSpeedCommand { get; set; }

        [DoNotNotify]
        [XmlIgnore]
        public ICommand PreStartHighSpeedCommand { get; set; }

        [DoNotNotify]
        [XmlIgnore]
        public ICommand StartHighSpeedCommand { get; set; }

        [DoNotNotify]
        [XmlIgnore]
        public ICommand StopHighSpeedCommand { get; set; }

        [DoNotNotify]
        [XmlIgnore]
        public ICommand FinalizeHighSpeedCommand { get; set; }

        /// <summary>
        /// If this controller is connected in high-speed mode
        /// </summary>
       [XmlIgnore]
        public bool IsConnectedHighSpeed
        {
            get { return _isConnectedHighSpeed; }
            set
            {
                _isConnectedHighSpeed = value;
                if (_isConnectedHighSpeed)
                {
                    ConnectHighSpeed();
                }
                else
                {
                    EnableLuminanceData = false;
                    DisconnectHighSpeed();
                }
            }
        }

        /// <summary>
        /// Whether to save image or not
        /// </summary>
        [XmlIgnore]
        public bool ShouldSaveImage { get; set; } = true;


        /// <summary>
        /// Status property
        /// </summary>
        [DoNotNotify]
        [XmlIgnore]
        public DeviceStatus Status
        {
            get { return _status; }
            set
            {
                _simpleArrayDataHighSpeed.Clear();
                CollectedRows = 0;
                _status = value;
            }
        }


        /// <summary>
        /// How many lines is needed to compose an image
        /// </summary>
        [XmlIgnore]
        public int RowsPerImage
        {
            get { return _rowsPerImage; }
            set
            {
                _rowsPerImage = value;
                CollectedRows = 0;
            }
        }

        [XmlIgnore]
        public bool EnableLuminanceData
        {
            get { return _simpleArrayDataHighSpeed.IsLuminanceEnable; }
            set { _simpleArrayDataHighSpeed.IsLuminanceEnable = value; }
        }

        /// <summary>
        /// How many rows within current image index has been collected
        /// </summary>
        [XmlIgnore]

        private int CollectedRows { get; set; }

        /// <summary>
        /// How many lines of profiles should be fetched for each data communication invoke
        /// </summary>
        public uint ProfileCountEachFetch { get; set; } = 100;

        /// <summary>
        /// The unique id managed by native library, defaults to the last byte of ip address
        /// </summary>
        private int _deviceId;

        [DoNotNotify]
        [XmlIgnore]
        public ushort HighSpeedPort { get; set; } = 24692;

        [DoNotNotify]
        [XmlIgnore]
        public string SerializationDirectory =>
            ApplicationViewModel.ApplicationViewModel.Instance.SerializationBaseDir;

        [XmlIgnore]
        public bool IsBufferFull { get; set; }

        /// <summary>Simple array data for high speed communication</summary>
        private readonly ProfileSimpleArrayStore _simpleArrayDataHighSpeed;

        private IpConfigViewModel.IpConfigViewModel _ipConfig;

        /// <summary>
        /// OK flag used by native library
        /// </summary>
        private static int _okFlag = (int) Rc.Ok;


        public event Action<HImage, HImage> ImageReady;

        /// <summary>
        /// Connect to the controller
        /// </summary>
        public void Connect()
        {
            var ip = IpConfig.ToNative();
            // Bind device id to this class or rather this ip
            var success = NativeMethods.LJX8IF_EthernetOpen(_deviceId, ref ip) == _okFlag;
            if (!success) Log($"Open connection failed at {IpConfig}");

            Status = success ? DeviceStatus.Ethernet : DeviceStatus.NoConnection;
            IpConfig = ip.ToViewModel();
        }

        public void InitHighSpeedCommunicationSimpleArray()
        {
            ThreadSafeBuffer.ClearBuffer(_deviceId); //Clear the retained profile data.
            _simpleArrayDataHighSpeed.Clear();
            var nativeIp = IpConfig.ToNative();
            var success = NativeMethods.LJX8IF_InitializeHighSpeedDataCommunicationSimpleArray(_deviceId, ref nativeIp,
                              HighSpeedPort, _callbackSimpleArray, ProfileCountEachFetch, (uint) _deviceId) == _okFlag;
            if (success)
            {
                Status = DeviceStatus.EthernetFast;
            }
        }


        /// <summary>
        /// This will be called when the controller scan every <see cref="ProfileCountEachFetch"/> lines of profiles
        /// </summary>
        /// <param name="headBuffer"></param>
        /// <param name="profileBuffer"></param>
        /// <param name="luminanceBuffer"></param>
        /// <param name="isLuminanceEnable"></param>
        /// <param name="profileSize"></param>
        /// <param name="count"></param>
        /// <param name="notify"></param>
        /// <param name="user"></param>
        private void CallBackSimpleArray(IntPtr headBuffer, IntPtr profileBuffer, IntPtr luminanceBuffer,
            uint isLuminanceEnable, uint profileSize, uint count, uint notify, uint user)
        {
            IsBufferFull = _simpleArrayDataHighSpeed.AddReceivedData(profileBuffer, luminanceBuffer, count);
            _simpleArrayDataHighSpeed.Notify = notify;
            CollectedRows += (int) count;
            if (CollectedRows == RowsPerImage)
            {
                CollectedRows = 0;
                Directory.CreateDirectory(SerializationDirectory);
                if (_simpleArrayDataHighSpeed.profileData.Count == 0)
                {
                    Log("Error: profileData.Count == 0 on image ready ");
                    return;
                }

                var heightImage = ToHImage(_simpleArrayDataHighSpeed.profileData.ToArray(),
                    _simpleArrayDataHighSpeed.DataWidth, RowsPerImage);
                var intensityImage = EnableLuminanceData ? 
                    ToHImage(_simpleArrayDataHighSpeed.luminanceData.ToArray(), _simpleArrayDataHighSpeed.DataWidth, RowsPerImage)
                    : null;
                OnImageReady(heightImage, intensityImage);

                _simpleArrayDataHighSpeed.Clear();
            }
        }





        private string HeightImageDir => SerializationDirectory + $"/{IpConfig.ForthByte}/";
        private string IntensityImageDir => SerializationDirectory + $"/{IpConfig.ForthByte}-Intensity/";


        public void PreStartHighSpeedCommunication()
        {
            // See native library why 2
            var request = new LJX8IF_HIGH_SPEED_PRE_START_REQUEST {bySendPosition = 2};

            LJX8IF_PROFILE_INFO profileInfo = new LJX8IF_PROFILE_INFO();

            var success =
                NativeMethods.LJX8IF_PreStartHighSpeedDataCommunication(_deviceId, ref request, ref profileInfo) ==
                _okFlag;
            if (!success)
            {
                Log("Fail to pre-start high-speed communication");
                return;
            }

            _simpleArrayDataHighSpeed.Clear();
            _simpleArrayDataHighSpeed.DataWidth = profileInfo.nProfileDataCount;
            // profileInfo.byLuminanceOutput == 1 => The controller was set to output luminance image
//            ShouldSaveLuminanceData = profileInfo.byLuminanceOutput == 1;
        }

        public void StartHighSpeedCommunication()
        {
            ThreadSafeBuffer.ClearBuffer(_deviceId);
            IsBufferFull = false;

            var success = NativeMethods.LJX8IF_StartHighSpeedDataCommunication(_deviceId) == _okFlag;
            if (!success)
            {
                Log("Failed to start high speed communication");
            }
        }


        public void StopHighSpeedCommunication()
        {
            var success = NativeMethods.LJX8IF_StopHighSpeedDataCommunication(_deviceId) == _okFlag;
            if (!success)
            {
                Log("Failed to stop high speed communication");
            }
        }

        public void FinalizeHighSpeedCommunication()
        {
            var success = NativeMethods.LJX8IF_FinalizeHighSpeedDataCommunication(_deviceId) == _okFlag;
            if (!success)
            {
                Log("Failed to finalize high speed communication");
            }

            if (Status == DeviceStatus.EthernetFast) Status = DeviceStatus.Ethernet;
        }

        public void Disconnect()
        {
            var success = NativeMethods.LJX8IF_CommunicationClose(_deviceId) == _okFlag;

            if (success) Status = DeviceStatus.NoConnection;
        }

        private void Serialize(HImage heightImage, HImage intensityImage)
        {
            var imageName = DateTime.Now.ToString("MMdd-HHmmss-ffff") + ".tif";
            if (!ShouldSaveImage) return;

            if (heightImage != null)
            {
                Directory.CreateDirectory(HeightImageDir);
                heightImage.WriteImage("tiff", 0, Path.Combine(HeightImageDir, imageName));
            }

            if (intensityImage != null)
            {
                Directory.CreateDirectory(IntensityImageDir);
                intensityImage.WriteImage("tiff", 0, Path.Combine(IntensityImageDir, imageName));
            }
        }


        /// <summary>
        /// Convert ushort array to <see cref="HImage"/>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        private HImage ToHImage(ushort[] data, int width, int height)
        {
            GCHandle pinnedArray = GCHandle.Alloc(data, GCHandleType.Pinned);
            IntPtr pointer = pinnedArray.AddrOfPinnedObject();


            HImage image = new HImage("uint2", width, height, pointer);
            pinnedArray.Free();
            return image;
        }

        private void CopyToImage(ushort[] data, HImage image, int width, int height)
        {
            image.GenImageConst("uint2", width, height);
            for (int row = 0; row < height; row++)
            {
                for (int col = 0; col < width; col++)
                {
                    image.SetGrayval(row, col, data[width * row + col]);
                }
            }
        }





        private static void Log(string msg)
        {
            IoC.IoC.Log(msg);
        }


      

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ControllerViewModel()
        {
            _simpleArrayDataHighSpeed = new ProfileSimpleArrayStore();
            _callbackSimpleArray = CallBackSimpleArray;

            ConnectCommand = new RelayCommand(Connect);
            DisconnectCommand = new RelayCommand(Disconnect);
            InitHighSpeedCommand = new RelayCommand(InitHighSpeedCommunicationSimpleArray);
            PreStartHighSpeedCommand = new RelayCommand(PreStartHighSpeedCommunication);
            StartHighSpeedCommand = new RelayCommand(StartHighSpeedCommunication);
            StopHighSpeedCommand = new RelayCommand(StopHighSpeedCommunication);
            FinalizeHighSpeedCommand = new RelayCommand(FinalizeHighSpeedCommunication);

            ImageReady += (heightImage, intensityImage) => { DisplayImageInvoke(heightImage); };
            ImageReady += Serialize;
        }

        private void DisplayImageInvoke(HImage heightImage)
        {
            var controllerName = Name;
            var imageList = ApplicationViewModel.ApplicationViewModel.Instance.AllImagesToShow;
            var visualization = heightImage;

            lock (ApplicationViewModel.ApplicationViewModel.Instance.LockerOfAllImagesToShow)
            {
                var newImageInfo = new ImageInfoViewModel()
                {
                    ControllerName = controllerName,
                    Image = visualization
                };

                var displayListHasMyImage = imageList.Any(ele => ele.ControllerName == controllerName);

                if (displayListHasMyImage)
                {
                    var previousImageInfo = imageList.First(ele => ele.ControllerName == controllerName);
                    // Remove displayed image if should not display
                    if (!ShouldImageBeDisplayed) imageList.Remove(previousImageInfo);
                    // Else update the displayed image
                    else
                    {
                        var myIndexInImageList = imageList.IndexOf(previousImageInfo);
                        imageList[myIndexInImageList] = newImageInfo;
                    }
                }

                if (!ShouldImageBeDisplayed || displayListHasMyImage) return;
                
                imageList.Add(newImageInfo);
                ApplicationViewModel.ApplicationViewModel.Instance.AllImagesToShow =
                    new ObservableCollection<ImageInfoViewModel>(imageList.OrderBy(ele => ele.ControllerName));

            }
        }


        /// <summary>
        /// From stop to disconnect in one step
        /// </summary>
        private void DisconnectHighSpeed()
        {
            StopHighSpeedCommunication();
            FinalizeHighSpeedCommunication();
            Disconnect();
        }

        /// <summary>
        /// Connect directly into high speed mode
        /// </summary>
        private void ConnectHighSpeed()
        {
            Connect();
            InitHighSpeedCommunicationSimpleArray();
            PreStartHighSpeedCommunication();
            StartHighSpeedCommunication();
        }

        #endregion

        #region Method

        /// <summary>
        /// Connection status acquisition
        /// </summary>
        /// <returns>Connection status for display</returns>
        public string GetStatusString()
        {
            string status = _status.ToString();
            switch (_status)
            {
                case DeviceStatus.Ethernet:
                case DeviceStatus.EthernetFast:
                    status += string.Format("---{0}.{1}.{2}.{3}", IpConfig.FirstByte, IpConfig.SecondByte,
                        IpConfig.ThirdByte, IpConfig.ForthByte);
                    break;
            }

            return status;
        }

        #endregion
    }
}