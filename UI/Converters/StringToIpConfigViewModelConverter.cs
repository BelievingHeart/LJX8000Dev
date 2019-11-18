using System;
using System.Globalization;
using LJX8000.Core.ViewModels.IpConfig;

namespace UI.Converters
{
    public class StringToIpConfigViewModelConverter : ValueConverterBase<StringToIpConfigViewModelConverter>
    {
        /// <summary>
        /// Receive ip config and convert to string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            return value.ToString();
        }

        /// <summary>
        /// Receive string and convert to ip config
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var text = (string)value;
            IpConfigViewModel ipConfig = text;
            return ipConfig;
        }
    }
}