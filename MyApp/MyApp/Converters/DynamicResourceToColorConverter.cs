using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace MyApp.Converters
{
    
    [Preserve(AllMembers = true)]
    public class DynamicResourceToColorConverter : IValueConverter
    {
        /// <summary>
        /// Конвертирует динамический ресурс в цвет
        /// </summary>
        /// <param name="value">Gets the value.</param>
        /// <param name="targetType">Gets the target type.</param>
        /// <param name="parameter">Gets the parameter.</param>
        /// <param name="culture">Gets the culture.</param>
        /// <returns>Returns the color.</returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is DynamicResource dynamicResource))
            {
                return value;
            }

            Application.Current.Resources.TryGetValue(dynamicResource.Key, out var color);
            return (Color)color;
        }

        /// <summary>
        /// Конвертирует цвет в динамический ресурс
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}