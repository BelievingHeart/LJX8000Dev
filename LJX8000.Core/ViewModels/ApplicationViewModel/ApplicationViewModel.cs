using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Data;
using HalconDotNet;
using LJX8000.Core.ViewModels.Base;
using LJX8000.Core.ViewModels.ControllerViewModel;
using LJX8000.Core.ViewModels.ImageInfo;
using MaterialDesignThemes.Wpf;

namespace LJX8000.Core.ViewModels.ApplicationViewModel
{
    public class ApplicationViewModel : ViewModelBase
    {
        private static ApplicationViewModel _Instance = new ApplicationViewModel()
        {
            MessageQueue = new SnackbarMessageQueue(TimeSpan.FromMilliseconds(5000)),
            LogRecords = new ObservableCollection<SideBarMessageItemViewModel.SideBarMessageItemViewModel>()
        };

        private string _serializationBaseDir = String.Empty;

        /// <summary>
        /// Application wide instance for xaml to bind to
        /// </summary>
        public static ApplicationViewModel Instance
        {
            get { return _Instance; }
        }

        public ApplicationViewModel()
        {
            BindingOperations.EnableCollectionSynchronization(AllImagesToShow, LockerOfAllImagesToShow);
        }
        
        #region Properties

        /// <summary>
        /// Message queue for ui logging
        /// </summary>
        public ISnackbarMessageQueue MessageQueue { get; set; }

        /// <summary>
        /// Items source for the side bar logging system
        /// </summary>
        public ObservableCollection<SideBarMessageItemViewModel.SideBarMessageItemViewModel> LogRecords { get; set; }

        public string SerializationBaseDir        
        {
            get { return _serializationBaseDir; }
            set { _serializationBaseDir = value; }
        }

        /// <summary>
        /// Maximum number of messages to show on side bar
        /// </summary>
        public int MaxMessages { get; set; } = 300;




        /// <summary>
        /// All the images that will be shown on screen
        /// </summary>
        public ObservableCollection<ImageInfoViewModel> AllImagesToShow { get; set; } = new ObservableCollection<ImageInfoViewModel>();

        /// <summary>
        /// Synchronization object for <see cref="AllImagesToShow"/>
        /// </summary>
        public object LockerOfAllImagesToShow { get;} = new object();


        public List<ControllerViewModel.ControllerViewModel> AttachedControllers => ControllerManager.AttachedControllers;

        

        #endregion
        /// <summary>
        /// Enqueue logging messages to the side bar
        /// </summary>
        /// <param name="msg"></param>
        public void Enqueue(SideBarMessageItemViewModel.SideBarMessageItemViewModel msg)
        {
            LogRecords.Add(msg);
            if(LogRecords.Count > MaxMessages) LogRecords.RemoveAt(0);
            AutoResetFlag = true;
        }
    }

}