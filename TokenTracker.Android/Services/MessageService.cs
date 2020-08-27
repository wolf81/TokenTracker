using Android.App;
using Android.Widget;
using TokenTracker.Services;
using TokenTracker.Services.Message;

[assembly: Xamarin.Forms.Dependency(typeof(MessageService))]
namespace TokenTracker.Services
{
    public class MessageService : IMessageService
    {
        public void Show(string message, DisplayDuration duration)
        {
            var toastLength = (duration == DisplayDuration.Long ? ToastLength.Long : ToastLength.Short);

            Xamarin.Forms.Device.BeginInvokeOnMainThread(() => {
                Toast.MakeText(Application.Context, message, toastLength).Show();
            });
        }
    }
}
