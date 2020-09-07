using System;
using System.Collections.Generic;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private List<SettingItemBase> items = new List<SettingItemBase>
        {
            new ChooserSettingItem { Title = "Currency", Choices = new List<string> { "USD", "EUR", "BTC" }, SelectedChoiceIndex = 0, IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_currency_b.png") },
            new ChooserSettingItem { Title = "Theme", Choices = new List<string> { "Light", "Dark", "Novel" }, SelectedChoiceIndex = 0, IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_theme_b.png") },
            new ChooserSettingItem { Title = "Sort By", Choices = new List<string> { "Alphabet", "Rank" }, SelectedChoiceIndex = 0, IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_sort_b.png") },
            new SwitchSettingItem { Title = "Disable Standby", IsSelected = false, IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_power_b.png") },
            new LabelSettingItem { Title = "Disable Ads", IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_block_b.png") },
            new LabelSettingItem { Title = "Clear Cache", IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_history_b.png") },
        };

        public List<SettingItemBase> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public SettingsViewModel()
        {
            Title = "Settings";
        }
    }
}
