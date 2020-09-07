using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace TokenTracker
{
    public abstract class SettingItemBase
    {
        public ImageSource IconImageSource { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }        
    }

    public class ChooserSettingItem : SettingItemBase
    {
        public int SelectedItemIndex { get; set; } = 0;

        public List<string> Items { get; set; } = new List<string> { };

        public Action<int> SelectedItemChanged;
    }

    public class SwitchSettingItem : SettingItemBase
    {
        public bool IsSelected { get; set; } = false;

        public Action<bool> SelectionChanged;
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

            throw new NotImplementedException();
        }
    }
}
