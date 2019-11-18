using System.Collections.Generic;
using LJX8000.Core.ViewModels.IpConfig;
using LJXNative;

namespace LJX8000.Core.ViewModels.Controller
{
    /// <summary>
    /// Manage all controllers
    /// </summary>
    public static class ControllerManager
    {
        private static void Log(string msg)
        {
            IoC.IoC.Log(msg);
        }

        /// <summary>
        /// OK flag used by native library
        /// </summary>
        private static int _okFlag = (int) Rc.Ok;
        
        public static List<IpConfigViewModel> ControllerIps { get; set; }

        public static List<Controller.ControllerViewModel> AttachedControllers { get; set; } = new List<ControllerViewModel>();

        /// <summary>
        /// Initiate the controller manage system
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
            if (ControllerIps == null)
            {
                Log("Failed to init the controller manager > ControllerIps has not set");
                return false;
            }

            var success = NativeMethods.LJX8IF_Initialize() == _okFlag;
            if (success)
            {
                foreach (var t in ControllerIps)
                {
                    AttachedControllers.Add( new Controller.ControllerViewModel(){ Name = t.ToString()});
                }

                return true;
            }
            
            Log("Failed to init the controller manager");
            return false;
        }
        

        public static int ControllerCount => ControllerIps.Count;
    }
}