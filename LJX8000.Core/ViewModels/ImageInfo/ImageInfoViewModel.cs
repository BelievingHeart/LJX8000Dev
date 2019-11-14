using HalconDotNet;

namespace LJX8000.Core.ViewModels.ImageInfo
{
    public class ImageInfoViewModel : ViewModelBase
    {
        public string ControllerName { get; set; }

        public HImage Image { get; set; }
    }
}