using System.Collections.Generic;
using System.Windows;
using LJX8000.Core.IoC;
using LJX8000.Core.IoC.Interface;
using LJX8000.Core.ViewModels.ControllerViewModel;
using LJX8000.Core.ViewModels.IpConfigViewModel;
using UI.DataAccess;

namespace UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <summary>
        /// Set up IoC
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set up controller manager
            ControllerManager.ControllerIps = new List<IpConfigViewModel>()
            {
                new IpConfigViewModel(){ForthByte = 1},
                new IpConfigViewModel(){ForthByte = 2},
                new IpConfigViewModel(){ForthByte = 3},
            };
            ControllerManager.Init();
            
            // Set up IoC
            IoC.Kernel.Bind<IUILogger>().ToConstant(new SidebarLogger());

            IoC.Setup();
            
            // Open main window
            var window = new MainWindow();
            Current.MainWindow = window;
            window.Show();
        }
    }
}