using System;
using System.Globalization;
using Xamarin.Forms;

namespace TokenTracker.Converters.Base
{
    public abstract class EnumToBooleanConverterBase<T> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is T type)
            {
                return type.Equals(GetParameter(parameter));
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public abstract T GetParameter(object parameter);
    }
}
