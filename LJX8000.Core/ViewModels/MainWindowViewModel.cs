using System.Collections.Generic;
using System.Linq;
using LJX8000.Core.ViewModels.ControllerViewModel;

namespace LJX8000.Core.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        /// <summary>
        /// Controller names by ip address
        /// </summary>
        public IEnumerable<string> ControllerNames => ControllerManager.AttachedControllers.Select(c => c.IpConfig.ToString());

        /// <summary>
        /// Name of the selected controller
        /// </summary>
        public string CurrentControllerName { get; set; }
    }
}