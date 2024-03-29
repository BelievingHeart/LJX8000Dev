﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows.Threading;
using LJX8000.Core.Commands;
using LJX8000.Core.ViewModels.Application;
using LJX8000.Core.ViewModels.Base;
using LJX8000.Core.ViewModels.Controller;
using MaterialDesignThemes.Wpf;

namespace LJX8000.Core.ViewModels
{
    public class ControllerHostViewModel : ViewModelBase
    {
        private bool _isAllConnected;
        private bool _shouldSaveAllLuminanceData;
        private DispatcherTimer _timer;
        private bool _isCollectingImagesDone;
        private int _maxImageSetsToCollect;

        /// <summary>
        /// Controller names by ip address
        /// </summary>
        public IEnumerable<string> ControllerNames => ControllerManager.AttachedControllers.Select(c => c.Name);

        /// <summary>
        /// Name of the selected controller
        /// </summary>
        public string CurrentControllerName { get; set; }

        public ICommand OpenImageDirCommand { get; set; }

        public ControllerHostViewModel()
        {
            OpenImageDirCommand = new RelayCommand(OpenImageDir);
            NextDirDialogCloseCallback = OnNextDirDialogClosing;
            _timer = new DispatcherTimer(TimeSpan.FromMilliseconds(500), DispatcherPriority.Normal, OnTimerTicked, Dispatcher.CurrentDispatcher);
            _timer.Start();
        }

        private void OnNextDirDialogClosing(object sender, DialogClosingEventArgs eventargs)
        {
            var param = (string) eventargs.Parameter;
            if (param == "ClickAway") MaxImageSetsToCollect = 0;
        }


        public bool ShouldSaveImages
        {
            get { return  ControllerManager.AttachedControllers.All(ele=>ele.ShouldSaveImage); }
            set
            {
                foreach (var controller in ControllerManager.AttachedControllers)
                {
                    controller.ShouldSaveImage = value;
                }
            }
        }


        public bool IsAllConnected    
        {
            get { return _isAllConnected; }
            set
            {
                _isAllConnected = value;
                if (_isAllConnected)
                {
                    ConnectAllHighSpeed();
                }
                else
                {
                    DisconnectAllHighSpeed();
                    ShouldSaveAllLuminanceData = false;
                }
            }
        }

        public bool ShouldSaveAllLuminanceData
        {
            get { return _shouldSaveAllLuminanceData; }
            set { _shouldSaveAllLuminanceData = value;
             if(_shouldSaveAllLuminanceData) EnableAllLuminance();
             else DisableAllLuminance();
            }
        }

        /// <summary>
        /// How many sets of images are collected in current directory
        /// </summary>
        public int NumImageSetsCollected { get; set; }


        /// <summary>
        /// How many image sets are going to be collected
        /// </summary>
        public int MaxImageSetsToCollect
        {
            get { return _maxImageSetsToCollect; }
            set
            {
                _maxImageSetsToCollect = value;
                NumImageSetsCollected = 0;
            }
        }

        public bool IsCollectingImagesDone
        {
            get { return _isCollectingImagesDone; }
            set
            {
                _isCollectingImagesDone = value;
                if (_isCollectingImagesDone)
                {
                    ShouldSaveImages = false;
                }
            }
        }

        public string ImageDirectoryName
        {
            set
            {
                if(string.IsNullOrEmpty(value)) return;
                // Reset image serialization directory
                var parentDir =
                    Directory.GetParent(ApplicationViewModel.Instance.SerializationBaseDir).FullName;
                ApplicationViewModel.Instance.SerializationBaseDir =
                    Path.Combine(parentDir, value);
                // Enable image serialization if image directory is updated
                ShouldSaveImages = true;
            }
        }

        public DialogClosingEventHandler NextDirDialogCloseCallback { get; }


        private void OnTimerTicked(object sender, EventArgs e)
        {
            CountSavedImages();
        }

        private void CountSavedImages()
        {
            var imageDir = ApplicationViewModel.Instance.SerializationBaseDir;
            if(!Directory.Exists(imageDir)) return;

            var subImageDirs = Directory.GetDirectories(imageDir);
            if (subImageDirs.Length == 0) return;

            var numSubImageDirs = subImageDirs.Length;
            var imageCountInLastSubDir = Directory.GetFiles(subImageDirs[numSubImageDirs-1]).Length;
            NumImageSetsCollected = imageCountInLastSubDir;
            var imageCountsAreEqual = subImageDirs.All(ele => Directory.GetFiles(ele).Length == imageCountInLastSubDir);
            
            if (MaxImageSetsToCollect <= 0) return;
            if(imageCountsAreEqual && NumImageSetsCollected == MaxImageSetsToCollect)
            {
                IsCollectingImagesDone = true;
            }
            

        }

    



        private void OpenImageDir()
        {
          
            var dir = ApplicationViewModel.Instance.SerializationBaseDir;
            if (string.IsNullOrEmpty(dir)) return;
            Directory.CreateDirectory(dir);

            try
            {
                Process.Start(dir);
            }
            catch
            {
                IoC.IoC.Log($"Directory:{dir} is not valid");
            }
        }
        
        private void DisconnectAllHighSpeed()
        {
            foreach (var controller in ControllerManager.AttachedControllers)
            {
                controller.IsConnectedHighSpeed = false;
            }
        }

        private void ConnectAllHighSpeed()
        {
            foreach (var controller in ControllerManager.AttachedControllers)
            {
                controller.IsConnectedHighSpeed = true;
            }
        }

        private void EnableAllLuminance()
                 {
                     foreach (var controller in ControllerManager.AttachedControllers)
                     {
                         controller.EnableLuminanceData = true;
                     }
                 }
        
        private void DisableAllLuminance()
        {
            foreach (var controller in ControllerManager.AttachedControllers)
            {
                controller.EnableLuminanceData = false;
            }
        }
    }
}