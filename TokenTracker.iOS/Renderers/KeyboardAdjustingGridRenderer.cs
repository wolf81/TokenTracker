using UIKit;
using Foundation;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using TokenTracker.Controls;
using TokenTracker.Renderers;

[assembly: ExportRenderer(typeof(KeyboardAdjustingGrid), typeof(KeyboardAdjustingGridRenderer))]
namespace TokenTracker.Renderers
{
    public class KeyboardAdjustingGridRenderer : ViewRenderer
    {
        private NSObject keyboardShowObserver;
        private NSObject keyboardHideObserver;

        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                RegisterForKeyboardNotifications();
            }

            if (e.OldElement != null)
            {
                UnregisterForKeyboardNotifications();
            }
        }

        #region Private

        private void RegisterForKeyboardNotifications()
        {
            if (keyboardShowObserver == null)
            {
                keyboardShowObserver = UIKeyboard.Notifications.ObserveWillShow(Handle_KeyboardDidShow);
            }

            if (keyboardHideObserver == null)
            {
                keyboardHideObserver = UIKeyboard.Notifications.ObserveWillHide(Handle_KeyboardDidHide);
            }
        }

        private void UnregisterForKeyboardNotifications()
        {
            if (keyboardShowObserver != null)
            {
                keyboardShowObserver.Dispose();
                keyboardShowObserver = null;
            }

            if (keyboardHideObserver != null)
            {
                keyboardHideObserver.Dispose();
                keyboardHideObserver = null;
            }
        }

        private void Handle_KeyboardDidShow(object sender, UIKeyboardEventArgs args)
        {
            var result = (NSValue)args.Notification.UserInfo.ObjectForKey(new NSString(UIKeyboard.FrameEndUserInfoKey));
            var keyboardSize = result.RectangleFValue.Size;

            if (Element != null)
            {
                Element.Margin = new Thickness(0, 0, 0, keyboardSize.Height); // push the entry up to keyboard height when keyboard is activated
            }
        }

        private void Handle_KeyboardDidHide(object sender, UIKeyboardEventArgs args)
        {
            if (Element != null)
            {
                Element.Margin = new Thickness(0); // set the margins to zero when keyboard is dismissed
            }
        }

        #endregion
    }
}
