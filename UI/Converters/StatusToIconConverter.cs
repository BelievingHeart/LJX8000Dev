using System;
using System.Globalization;
using LJX8000.Core.Enums;

namespace UI.Converters
{
    public class StatusToIconConverter : ValueConverterBase<StatusToIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var status = (DeviceStatus) value;
            switch (status)
            {
                case DeviceStatus.NoConnection:
                    return "LocalAreaNetworkDisconnect";
                case DeviceStatus.Ethernet:
                    return "LanConnect";
                default:
                    return "Rocket";
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}