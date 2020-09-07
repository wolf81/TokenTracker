using System.Collections.Generic;
using Xamarin.Forms;

namespace TokenTracker
{
    public abstract class SettingItemBase
    {
        public ImageSource IconImageSource { get; set; }

        public string Title { get; set; }
    }

    public class ChooserSettingItem : SettingItemBase
    {
        public int SelectedChoiceIndex { get; set; } = 0;

        public List<string> Choices { get; set; } = new List<string> { "Choice 1" };
    }

    public class SwitchSettingItem : SettingItemBase
    {
        public bool IsSelected { get; set; } = false;
    }

    public class LabelSettingItem: SettingItemBase { }

    public class SettingTemplateSelector : DataTemplateSelector
    {
        public DataTemplate ChooserTemplate { get; set; }

        public DataTemplate SwitchTemplate { get; set; }

        public DataTemplate LabelTemplate { get; set; }
        
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (item is SwitchSettingItem)
            {
                return SwitchTemplate;
            }
            else if (item is ChooserSettingItem)
            {
                return ChooserTemplate;
            }
            else if (item is LabelSettingItem)
            {
                return LabelTemplate;
            }

            throw new System.NotImplementedException();
        }
    }
}
