using System;
using System.Collections.Generic;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker
{
    public abstract class SettingItemBase : ExtendedBindableObject
    {
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }

        private bool isBusy;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }

        private ImageSource iconImageSource;
        public ImageSource IconImageSource
        {
            get => iconImageSource;
            set => SetProperty(ref iconImageSource, value);
        }

        private string description;
        public string Description
        {
            get => description;
            set => SetProperty(ref description, value);
        }
    }

    public class ChooserSettingItem : SettingItemBase
    {
        private int selectedItemIndex = 0;
        public int SelectedItemIndex
        {
            get => selectedItemIndex;
            set => SetProperty(ref selectedItemIndex, value);
        }

        private List<string> items = new List<string> { };
        public List<string> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public Action<int> SelectedItemChanged;
    }

    public class SwitchSettingItem : SettingItemBase
    {
        private bool isSelected;
        public bool IsSelected
        {
            get => isSelected;
            set => SetProperty(ref isSelected, value);
        } 

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
