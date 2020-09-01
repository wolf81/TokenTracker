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
                    case "LoadingState.None": return LoadingState.None;
                    case "LoadingState.Loading": return LoadingState.Loading;
                    case "LoadingState.Finished": return LoadingState.Finished;
                    case "LoadingState.Failed": return LoadingState.Failed;
                }
            }

            return LoadingState.None;
        }
    }
}
