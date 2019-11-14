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
            AdaptRatio(ActualWidth);
            
            SizeChanged += (ss, ee) =>
            {
                var currentWidth = ee.NewSize.Width;
                AdaptRatio(currentWidth);
            };
        }

        private void AdaptRatio(double currentWidth)
        {
            var dataContext = DataContext as ImageInfoViewModel;
            var ratio = dataContext.ImageWidthRatio;
            
            Height = currentWidth * ratio;
        }

    }
}