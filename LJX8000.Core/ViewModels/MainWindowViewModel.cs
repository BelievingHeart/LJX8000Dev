﻿using System.Collections.Generic;
using System.Linq;
using LJX8000.Core.ViewModels.ControllerViewModel;

namespace LJX8000.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _isAllConnected;
        private bool _shouldSaveAllLuminanceData;

        /// <summary>
        /// Controller names by ip address
        /// </summary>
        public IEnumerable<string> ControllerNames => ControllerManager.AttachedControllers.Select(c => c.IpConfig.ToString());

        /// <summary>
        /// Name of the selected controller
        /// </summary>
        public string CurrentControllerName { get; set; }

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
                         controller.ShouldSaveLuminanceData = true;
                     }
                 }
        
        private void DisableAllLuminance()
        {
            foreach (var controller in ControllerManager.AttachedControllers)
            {
                controller.ShouldSaveLuminanceData = false;
            }
        }
    }
}