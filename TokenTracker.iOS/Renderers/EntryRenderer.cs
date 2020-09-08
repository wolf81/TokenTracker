using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(Entry), typeof(TokenTracker.Renderers.EntryRenderer))]
namespace TokenTracker.Renderers
{
    public class EntryRenderer : Xamarin.Forms.Platform.iOS.EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control is UITextField textField)
            {
                textField.AutocorrectionType = UITextAutocorrectionType.No;
                textField.SpellCheckingType = UITextSpellCheckingType.No;
                textField.AutocapitalizationType = UITextAutocapitalizationType.None;
            }
        }
    }
}
