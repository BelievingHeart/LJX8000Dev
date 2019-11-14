using System.Windows;
using System.Windows.Controls;

namespace UI.Views.ImageInfo
{
    public partial class ImageInfoView : UserControl
    {
        public ImageInfoView()
        {
            InitializeComponent();
        }

        private void ImageInfoView_OnLoaded(object sender, RoutedEventArgs e)
        {
            SmartWindowControlWpf.HalconWindow.SetLut("twelve");
        }
    }
}