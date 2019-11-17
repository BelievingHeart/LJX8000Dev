using HalconDotNet;
using LJX8000.Core.ViewModels.Base;

namespace LJX8000.Core.ViewModels.ImageInfo
{
    public class ImageInfoViewModel : ViewModelBase
    {
        private HImage _image;
        public string ControllerName { get; set; }

        public HImage Image    
        {
            get { return _image; }
            set
            {
                _image = value;
                HTuple width, height;
                _image.GetImageSize(out width, out height);
                ImageWidthRatio = (double) height/(double) width;
            }
        }
        

        /// <summary>
        /// image height / image width
        /// </summary>
        public double ImageWidthRatio { get; set; } = 1;
        
    }
}