using System.Collections.Generic;
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
        

        public static List<ControllerViewModel> AttachedControllers { get; set; } = new List<ControllerViewModel>();

        /// <summary>
        /// Initiate the controller manage system
        /// </summary>
        /// <returns></returns>
        public static bool Init()
        {
        

            var success = NativeMethods.LJX8IF_Initialize() == _okFlag;
            if (success)
            {
                return true;
            }
            
            Log("Failed to init the controller manager");
            return false;
        }
        

    }
}