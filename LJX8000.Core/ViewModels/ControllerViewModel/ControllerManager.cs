using System.Collections.Generic;
using System.Collections.ObjectModel;
using LJX8000.Core.Helpers;
using LJXNative;
using LJXNative.Data;

namespace LJX8000.Core.ViewModels.ControllerViewModel
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
        
        public static List<IpConfigViewModel.IpConfigViewModel> ControllerIps { get; set; }

        public static List<ControllerViewModel> AttachedControllers { get; set; } = new List<ControllerViewModel>();

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
                    AttachedControllers.Add( new ControllerViewModel(){ Name = t.ToString()});
                }

                return true;
            }
            
            Log("Failed to init the controller manager");
            return false;
        }
        

        public static int ControllerCount => ControllerIps.Count;
    }
}