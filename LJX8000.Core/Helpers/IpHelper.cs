using LJX8000.Core.ViewModels.IpConfigViewModel;
using LJXNative;

namespace LJX8000.Core.Helpers
{
    public static class IpHelper
    {
        public static IpConfigViewModel ToViewModel(this LJX8IF_ETHERNET_CONFIG config)
        {
            return new IpConfigViewModel()
            {
                FirstByte = config.abyIpAddress[0],
                SecondByte = config.abyIpAddress[1],
                ThirdByte = config.abyIpAddress[2],
                ForthByte = config.abyIpAddress[3],
                Port = config.wPortNo
            };
        }

        public static LJX8IF_ETHERNET_CONFIG ToNative(this IpConfigViewModel model)
        {
            var bytes = new byte[4];
            bytes[0] = model.FirstByte;
            bytes[1] = model.SecondByte;
            bytes[2] = model.ThirdByte;
            bytes[3] = model.ForthByte;
            
            return new LJX8IF_ETHERNET_CONFIG()
            {
                abyIpAddress = bytes,
                wPortNo = model.Port
            };
        }
    }
}