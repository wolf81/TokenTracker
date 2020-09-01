using TokenTracker.Models;

namespace TokenTracker.Converters
{
    public class DisplayModeToBooleanConverter : Base.EnumToBooleanConverterBase<DisplayMode>
    {
        public override DisplayMode GetParameter(object parameter)
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
    }
}
