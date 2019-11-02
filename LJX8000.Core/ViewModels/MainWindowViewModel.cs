using System.Collections.Generic;
using System.Linq;
using LJX8000.Core.ViewModels.ControllerViewModel;

namespace LJX8000.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private bool _isAllConnected;

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
                }
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
    }
}