using System;
using System.Globalization;
using TokenTracker.Models;
using Xamarin.Forms;

namespace TokenTracker.Converters
{
    public class DisplayModeToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DisplayMode displayMode)
            {
                return displayMode == GetParameter(parameter);
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #region Private

        private DisplayMode GetParameter(object parameter)
        {
            if (parameter is string)
            {
                switch (parameter)
                {
                    case "DisplayMode.Edit": return DisplayMode.Edit;
                    case "DisplayMode.View": return DisplayMode.View;
                }
            }

            return DisplayMode.View;
        }

        #endregion
    }
}
