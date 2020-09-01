using TokenTracker.Models;

namespace TokenTracker.Converters
{
    public class LoadingStateToBooleanConverter : Base.EnumToBooleanConverterBase<LoadingState>
    {
        public override LoadingState GetParameter(object parameter)
        {            
            if (parameter is string)
            {
                switch (parameter)
                {
                    case "LoadingState.Done": return LoadingState.Done;
                    case "LoadingState.Empty": return LoadingState.Empty;
                    case "LoadingState.Error": return LoadingState.Error;
                    case "LoadingState.Loading": return LoadingState.Loading;
                }
            }

            return LoadingState.Empty;
        }
    }
}
