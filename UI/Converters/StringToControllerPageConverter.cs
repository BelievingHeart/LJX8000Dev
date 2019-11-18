using System;
using System.Globalization;
using System.Linq;
using LJX8000.Core.ViewModels.Controller;
using UI.Views.ContollerView;

namespace UI.Converters
{
    /// <summary>
    /// Convert string to a controller page
    /// </summary>
    public class StringToControllerPageConverter : ValueConverterBase<StringToControllerPageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var name = (string) value;
            if (name == null) return null;
            var viewModel = ControllerManager.AttachedControllers.First(c => c.Name == name);
            return new ControllerView
            {
                DataContext = viewModel
            };
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}