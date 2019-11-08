﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Net.Mime;
using System.Security.Policy;
using System.Threading.Tasks;
using System.Windows.Input;
using HalconDotNet;
using LJX8000.Core.Commands;
using LJX8000.Core.Enums;
using LJX8000.Core.Helpers;
using LJXNative;
using LJXNative.Data;

namespace LJX8000.Core.ViewModels.ControllerViewModel
{
    public class ControllerViewModel : ViewModelBase
    {
        public IpConfigViewModel.IpConfigViewModel IpConfig { get; set; }

        #region Field

        /// <summary>Connection state</summary>
        private DeviceStatus _status = DeviceStatus.NoConnection;

        /// <summary>
        /// Set as field so that it would not be garbage collected
        /// </summary>
        private HighSpeedDataCallBackForSimpleArray _callbackSimpleArray;

        private int _rowsPerImage = 2400;
        private bool _isConnectedHighSpeed = false;

        #endregion

        #region Property

        public ICommand ConnectCommand { get; set; }
        public ICommand DisconnectCommand { get; set; }
        public ICommand InitHighSpeedCommand { get; set; }
        public ICommand PreStartHighSpeedCommand { get; set; }
        public ICommand StartHighSpeedCommand { get; set; }
        public ICommand StopHighSpeedCommand { get; set; }
        public ICommand FinalizeHighSpeedCommand { get; set; }
        public ICommand OpenImageDirCommand { get; set; }

        /// <summary>
        /// If this controller is connected in high-speed mode
        /// </summary>
        public bool IsConnectedHighSpeed
        {
            get { return _isConnectedHighSpeed; }
            set
            {
                _isConnectedHighSpeed = value;
                if(_isConnectedHighSpeed)
                {
                    ConnectHighSpeed();
                }
                else
                {
                    ShouldSaveLuminanceData = false;
                    DisconnectHighSpeed();
                }
            }
        }


        /// <summary>
        /// Status property
        /// </summary>
        public DeviceStatus Status
        {
            get { return _status; }
            set
            {
                SimpleArrayDataHighSpeed.Clear();
                CollectedRows = 0;
                _status = value;
            }
        }



        /// <summary>
        /// How many lines is needed to compose an image
        /// </summary>
        public int RowsPerImage
        {
            get { return _rowsPerImage; }
            set
            {
                _rowsPerImage = value;
                CollectedRows = 0;
            }
        }

        public bool ShouldSaveLuminanceData {
            get { return SimpleArrayDataHighSpeed.IsLuminanceEnable; }
            set { SimpleArrayDataHighSpeed.IsLuminanceEnable = value; } }
        
        /// <summary>
        /// How many rows within current image index has been collected
        /// </summary>
        private int CollectedRows { get; set; }
        
        /// <summary>
        /// How many lines of profiles should be fetched for each data communication invoke
        /// </summary>
        public uint ProfileCountEachFetch { get; set; } = 100;

        /// <summary>
        /// The unique id managed by native library, defaults to the last byte of ip address
        /// </summary>
        public int DeviceId { get; set; }

        public ushort HighSpeedPort { get; set; } = 24692;

        /// <summary>
        /// OK flag used by native library
        /// </summary>
        private static int _okFlag = (int) Rc.Ok;

        /// <summary>
        /// Connect to the controller
        /// </summary>
        public void Connect()
        {
            var ip = IpConfig.ToNative();
            // Bind device id to this class or rather this ip
            var success = NativeMethods.LJX8IF_EthernetOpen(DeviceId, ref ip) == _okFlag;
            if (!success) Log($"Open connection failed at {IpConfig}");
       
            Status = success ? DeviceStatus.Ethernet : DeviceStatus.NoConnection;
            IpConfig = ip.ToViewModel();
        }

        public void InitHighSpeedCommunicationSimpleArray()
        {
            ThreadSafeBuffer.ClearBuffer(DeviceId); //Clear the retained profile data.
            SimpleArrayDataHighSpeed.Clear();
            var nativeIp = IpConfig.ToNative();
            var success = NativeMethods.LJX8IF_InitializeHighSpeedDataCommunicationSimpleArray(DeviceId, ref nativeIp,
                HighSpeedPort, _callbackSimpleArray, ProfileCountEachFetch, (uint) DeviceId) == _okFlag;
            if(success)
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
            IsBufferFull = SimpleArrayDataHighSpeed.AddReceivedData(profileBuffer, luminanceBuffer, count);
            SimpleArrayDataHighSpeed.Notify = notify;
            CollectedRows += (int)count;
            if (CollectedRows == RowsPerImage)
            {
                CollectedRows = 0;
                Directory.CreateDirectory(SerializationDirectory);
                    Serialize($"{SerializationDirectory}/{DateTime.Now.ToString("MMdd-HHmmss-ffff")}.tif",
                        0, RowsPerImage);
                    SimpleArrayDataHighSpeed.Clear();
                    
               
                
            }
        }

 

        public string SerializationDirectory => Directory.GetCurrentDirectory() + $"/Images/{IpConfig.ForthByte}/";

        private ICommand ResetCurrentImageIndexCommand { get; set; }

        public void PreStartHighSpeedCommunication()
        {
            // See native library why 2
            var request = new LJX8IF_HIGH_SPEED_PRE_START_REQUEST {bySendPosition = 2};

            LJX8IF_PROFILE_INFO profileInfo = new LJX8IF_PROFILE_INFO();

            var success =
                NativeMethods.LJX8IF_PreStartHighSpeedDataCommunication(DeviceId, ref request, ref profileInfo) ==
                _okFlag;
            if (!success)
            {
                Log("Fail to pre-start high-speed communication");
                return;
            }

            SimpleArrayDataHighSpeed.Clear();
            SimpleArrayDataHighSpeed.DataWidth = profileInfo.nProfileDataCount;
            // profileInfo.byLuminanceOutput == 1 => The controller was set to output luminance image
//            ShouldSaveLuminanceData = profileInfo.byLuminanceOutput == 1;
        }

        public void StartHighSpeedCommunication()
        {
            ThreadSafeBuffer.ClearBuffer(DeviceId);
            IsBufferFull = false;

            var success = NativeMethods.LJX8IF_StartHighSpeedDataCommunication(DeviceId) == _okFlag;
            if (!success)
            {
                Log("Failed to start high speed communication");
            }
         
        }
        
        

        public void StopHighSpeedCommunication()
        {
            var success = NativeMethods.LJX8IF_StopHighSpeedDataCommunication(DeviceId) == _okFlag;
            if (!success)
            {
                Log("Failed to stop high speed communication");
            }
        }

        public void FinalizeHighSpeedCommunication()
        {
            var success = NativeMethods.LJX8IF_FinalizeHighSpeedDataCommunication(DeviceId) == _okFlag;
            if (!success)
            {
                Log("Failed to finalize high speed communication");
            }

            if (Status == DeviceStatus.EthernetFast) Status = DeviceStatus.Ethernet;
        }

        public void Disconnect()
        {
            var success = NativeMethods.LJX8IF_CommunicationClose(DeviceId) == _okFlag;

            if (success) Status = DeviceStatus.NoConnection;
        }

        public void Serialize(string path, int start, int count)
        {
            if (SimpleArrayDataHighSpeed.DataWidth == 0)
            {
                Log("Failed to serialize > data width == 0");
                return;
            }

            SimpleArrayDataHighSpeed.SaveDataAsImages(path, start, count);
        }


        public bool IsBufferFull { get; set; }



        private static void Log(string msg)
        {
            IoC.IoC.Log(msg);
        }


        /// <summary>Simple array data for high speed communication</summary>
        public ProfileSimpleArrayStore SimpleArrayDataHighSpeed { get; }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ControllerViewModel()
        {
            SimpleArrayDataHighSpeed = new ProfileSimpleArrayStore();
            _callbackSimpleArray = CallBackSimpleArray;

            ConnectCommand = new RelayCommand(Connect);
            DisconnectCommand = new RelayCommand(Disconnect);
            InitHighSpeedCommand = new RelayCommand(InitHighSpeedCommunicationSimpleArray);
            PreStartHighSpeedCommand = new RelayCommand(PreStartHighSpeedCommunication);
            StartHighSpeedCommand = new RelayCommand(StartHighSpeedCommunication);
            StopHighSpeedCommand = new RelayCommand(StopHighSpeedCommunication);
            FinalizeHighSpeedCommand = new RelayCommand(FinalizeHighSpeedCommunication);
            OpenImageDirCommand = new RelayCommand(OpenImageDir);
            
            
            
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
        
        private void OpenImageDir()
        {
            Directory.CreateDirectory(SerializationDirectory);
            Process.Start(SerializationDirectory);
        }
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