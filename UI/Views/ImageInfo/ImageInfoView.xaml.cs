using System.Windows;
using System.Windows.Controls;
using HalconDotNet;
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
            SmartWindowControlWpf.HalconWindow.SetLut("twelve");
            SizeChanged += AdaptRatio;
        }

        private void AdaptRatio(object sender, SizeChangedEventArgs e)
        {
            var dataContext = DataContext as ImageInfoViewModel;
            var ratio = dataContext.ImageWidthRatio;
            var currentWidth = e.NewSize.Width;
            Height = currentWidth * ratio;
        }

    }
}