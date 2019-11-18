using System;
using System.Globalization;
using LJX8000.Core.Enums;
using UI.Views.ControllerConfiguration;
using UI.Views.ControllerHost;

namespace UI.Converters
{
    public class EnumToApplicationPageConverter : ValueConverterBase<EnumToApplicationPageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                var pageEnum = (ApplicationPage) value;
                switch (pageEnum)
                {
                    case ApplicationPage.ControllerHostPage:
                        return new ControllerHostView();
                    default:
                        return new ControllerConfigurationView();
                }
            }
            throw new NullReferenceException("Page enum can not be null");
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}