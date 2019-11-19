using System.Windows;
using System.Windows.Controls;

namespace UI.Views.ControllerConfiguration
{
    public partial class ControllerConfigurationView : UserControl
    {
        public ControllerConfigurationView()
        {
            InitializeComponent();
        }

        private void ControllerConfigurationView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.WindowStyle = WindowStyle.None;
            mainWindow.ResizeMode = ResizeMode.NoResize;
            mainWindow.MinHeight = 500;
            mainWindow.MinWidth = 400;
            mainWindow.Height = 500;
            mainWindow.Width = 400;
        }
    }
}