using System.Collections.Generic;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels.Base;
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
            SelectedItemIndex = 0,
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_currency_b.png"),
        };

        private readonly ChooserSettingItem themeItem = new ChooserSettingItem
        {
            Title = "Theme",
            Items = new List<string> { "Light", "Dark", "Novel" },
            SelectedItemIndex = 0,
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_theme_b.png"),
        };

        private readonly ChooserSettingItem sortByItem = new ChooserSettingItem
        {
            Title = "Sort By",
            Items = new List<string> { "Alphabet", "Rank" },
            SelectedItemIndex = 0,
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_sort_b.png"),
        };

        private SwitchSettingItem disableStandbyItem = new SwitchSettingItem
        {
            Title = "Disable Standby",
            IsSelected = false,
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_standby_b.png")
        };

        private LabelSettingItem removeAdsItem = new LabelSettingItem
        {
            Title = "Remove Ads",
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_block_b.png")
        };

        private LabelSettingItem clearCacheItem = new LabelSettingItem
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

            Items = new List<SettingItemBase> { currencyItem, themeItem, sortByItem, disableStandbyItem, removeAdsItem, clearCacheItem };

            currencyItem.SelectedItemChanged = OnCurrencyChanged;
            sortByItem.SelectedItemChanged = OnSortByChanged;
            themeItem.SelectedItemChanged = OnThemeChanged;
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

        #endregion
    }
}
