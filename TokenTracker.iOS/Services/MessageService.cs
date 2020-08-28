using TokenTracker.Extensions;
using TokenTracker.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(MessageService))]
namespace TokenTracker.Services
{
    public class MessageService : IMessageService
    {
        private const double LongDelay = 3.5f;
        private const double ShortDelay = 2.0f;

        public void Show(string message, DisplayDuration duration)
        {
            var keyWindow = UIApplication.SharedApplication.KeyWindow;
            keyWindow.ShowToast(message, duration == DisplayDuration.Long ? LongDelay : ShortDelay);
        }
    }
}
