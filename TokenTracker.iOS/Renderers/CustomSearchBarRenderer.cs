using TokenTracker.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(SearchBar), typeof(CustomSearchBarRenderer))]
namespace TokenTracker.Renderers
{
    public class CustomSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                Control.AutocorrectionType = UIKit.UITextAutocorrectionType.No;
                Control.AutocapitalizationType = UIKit.UITextAutocapitalizationType.None;
                Control.ReturnKeyType = UIKit.UIReturnKeyType.Done;                
            }
        }
    }
}
