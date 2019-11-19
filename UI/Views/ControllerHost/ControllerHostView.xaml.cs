using System.Windows;
using System.Windows.Controls;

namespace UI.Views.ControllerHost
{
    public partial class ControllerHostView : UserControl
    {
        public ControllerHostView()
        {
            InitializeComponent();
        }

        private void ControllerHostView_OnLoaded(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow) Application.Current.MainWindow;
            mainWindow.WindowState = WindowState.Maximized;
            mainWindow.WindowStyle = WindowStyle.SingleBorderWindow;
            mainWindow.ResizeMode = ResizeMode.CanResize;
            mainWindow.MinHeight = 800;
            mainWindow.MinWidth = 1000;
        }
    }
}