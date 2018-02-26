using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace MayeXpenses.Converters
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string s)
        {
            return CostConversions.FormatDate((DateTime)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string s)
        {
            throw new NotImplementedException();
        }
    }

    public class ColourConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string s)
        {
            return (float)value < 0 ? new SolidColorBrush(Colors.Green) : new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object valye, Type targetType, object parameter, string s)
        {
            throw new NotImplementedException();
        }
    }

    public class CostConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string s)
        {
            return CostConversions.FormatCost((float)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string s)
        {
            throw new NotImplementedException();
        }
    }

    public class CurrencyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string s)
        {
            return DefaultVars.currency[(int)value];
        }

        public object ConvertBack(object value, Type targetType, object paramter, string s)
        {
            throw new NotImplementedException();
        }
    }

    public class DescriptionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string s)
        {
            string v = value.ToString();
            return v.Length > 25 ? v.Substring(0, 25) + "..." : v;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string s)
        {
            throw new NotImplementedException();
        }
    }
}
