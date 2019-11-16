using System.Diagnostics;
using HalconDotNet;

namespace LJX8000.Core.Helpers
{
    public static class HalconHelpers
    {
        /// <summary>
        /// Scale range from (minPixelValue + epslon, maxPixelVale) to (0, 65535)
        /// </summary>
        /// <param name="image"></param>
        /// <param name="epslon"></param>
        /// <returns></returns>
        public static HImage ScaleValidRangeUShort(this HImage image, int epslon)
        {

            HTuple width, height, min, max, range;
            // Determine invalid range
            image.GetImageSize(out width, out height);
            var regionImage = new HRegion(0, 0, height, width);
            image.MinMaxGray(regionImage, 0, out min, out max, out range);

            // Select region within valid range
            var validRegion = image.Threshold(min + epslon, 99999);
            var validImage = image.ReduceDomain(validRegion);
            validImage.MinMaxGray(validRegion, 0, out min, out max, out range);

            // Normalize valid range
            var imageSub = validImage.ScaleImage(0, -min);
            double scaleFactor = 65535 / (max - min);
            var imageScale = imageSub.ScaleImage(scaleFactor, 0);

            return imageScale;
        }
    }
}