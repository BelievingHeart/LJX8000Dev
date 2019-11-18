using System.Windows;
using LJX8000.Core.Helpers;
using LJX8000.Core.IoC;
using LJX8000.Core.IoC.Interface;
using LJX8000.Core.ViewModels.Application;
using LJX8000.Core.ViewModels.Controller;
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

            var loadedControllers =
                AutoSerializableHelper.LoadAllAutoSerializables<ControllerViewModel>(ApplicationViewModel
                    .ControllerSerializationBaseDir);
            ApplicationViewModel.Instance.RememberedControllers.AddRange(loadedControllers);
            
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