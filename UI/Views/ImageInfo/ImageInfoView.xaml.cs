using System.Windows;
using System.Windows.Controls;
using LJX8000.Core.ViewModels.ImageInfo;

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
//            SmartWindowControlWpf.HalconWindow.SetLut("twelve");
            var dataContext = DataContext as ImageInfoViewModel;
            SmartWindowControlWpf.HalconWindow.DispImage(dataContext.Image);
        }
    }
}