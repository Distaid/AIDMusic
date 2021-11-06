using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace AIDMusicApp.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter != null && parameter.ToString() == "is_dead")
            {
                switch((bool)value)
                {
                    case true:
                        return Visibility.Visible;
                    case false:
                        return Visibility.Hidden;
                }
            }

            return Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
