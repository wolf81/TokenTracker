using System;
using System.Collections.Generic;
using System.Linq;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace TokenTracker.ViewModels
{
    public class SettingsViewModel : ViewModelBase
    {
        private ISettingsService Settings => ViewModelLocator.Resolve<ISettingsService>();

        private readonly ChooserSettingItem currencyItem = new ChooserSettingItem
        {
            Title = "Currency",
            Items = new List<string> { "USD", "EUR", "BTC" },
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_currency_b.png"),
        };

        private readonly ChooserSettingItem themeItem = new ChooserSettingItem
        {
            Title = "Theme",
            Items = new List<string> { "Light", "Dark", "Novel" },
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_theme_b.png"),
        };

        private readonly ChooserSettingItem sortByItem = new ChooserSettingItem
        {
            Title = "Sort By",
            Items = Enum.GetNames(typeof(SortOrder)).ToList(),
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_sort_b.png"),
        };

        private readonly SwitchSettingItem suspendSleepItem = new SwitchSettingItem
        {
            Title = "Suspend Sleep",
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_standby_b.png")
        };

        private readonly LabelSettingItem removeAdsItem = new LabelSettingItem
        {
            Title = "Remove Ads",
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_block_b.png")
        };

        private readonly LabelSettingItem clearCacheItem = new LabelSettingItem
        {
            Title = "Clear Cache",
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_history_b.png")
        };

        private List<SettingItemBase> items = new List<SettingItemBase> { };
        public List<SettingItemBase> Items
        {
            get => items;
            set => SetProperty(ref items, value);
        }

        public SettingsViewModel()
        {
            Title = "Settings";

            Items = new List<SettingItemBase> { currencyItem, themeItem, sortByItem, suspendSleepItem, removeAdsItem, clearCacheItem };

            currencyItem.SelectedItemChanged = OnCurrencyChanged;
            sortByItem.SelectedItemChanged = OnSortByChanged;
            themeItem.SelectedItemChanged = OnThemeChanged;
            suspendSleepItem.SelectionChanged = OnSuspendSleepChanged;
        }

        public void Update()
        {
            var sortItemName = Settings.SortOrder.ToString("g");
            sortByItem.SelectedItemIndex = sortByItem.Items.IndexOf(sortItemName);
            suspendSleepItem.IsSelected = DeviceDisplay.KeepScreenOn;
        }        

        #region Private

        private void OnSortByChanged(int selectedIndex)
        {
            Settings.SortOrder = selectedIndex == 0 ? SortOrder.Alphabet : SortOrder.Rank;
        }

        private void OnCurrencyChanged(int selectedIndex)
        {
            
        }

        private void OnThemeChanged(int selectedIndex)
        {
            
        }

        private void OnSuspendSleepChanged(bool isSuspendSleepEnabled)
        {
            DeviceDisplay.KeepScreenOn = isSuspendSleepEnabled;            
        }

        #endregion
    }
}
