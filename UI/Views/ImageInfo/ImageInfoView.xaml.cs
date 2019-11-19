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
            // Uncomment the following lines the colorize images
            
//            SmartWindowControlWpf.HalconWindow.SetLut("twelve");
//            // Display again after colorization took effects
//            var windowHandle = SmartWindowControlWpf.HalconWindow;
//            var imageDisplayed = (DataContext as ImageInfoViewModel)?.Image;
//            windowHandle.DispImage(imageDisplayed);

            AdaptRatio(ActualWidth);
            
            SizeChanged += (ss, ee) =>
            {
                var currentWidth = ee.NewSize.Width;
                AdaptRatio(currentWidth);
            };
        }

        private void AdaptRatio(double currentWidth)
        {
            var dataContext = (ImageInfoViewModel)DataContext;
            var ratio = dataContext.ImageWidthRatio;
            
            Height = currentWidth * ratio;
        }

    }
}