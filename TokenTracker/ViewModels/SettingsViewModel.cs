using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        private ICache Cache => ViewModelLocator.Resolve<ICache>();

        private readonly ChooserSettingItem currencyItem = new ChooserSettingItem
        {
            Title = "Currency",
            Items = new List<KeyValuePair<string, string>> { },
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_currency_b.png"),
        };

        private readonly ChooserSettingItem themeItem = new ChooserSettingItem
        {
            Title = "Theme",            
            Items = Enum.GetNames(typeof(Theme)).Select((t) => new KeyValuePair<string, string>(t, t)).ToList(),
            IconImageSource = ImageSource.FromResource("TokenTracker.Resources.ic_theme_b.png"),
        };

        private readonly ChooserSettingItem sortByItem = new ChooserSettingItem
        {
            Title = "Sort By",
            Items = Enum.GetNames(typeof(SortOrder)).Select((s) => new KeyValuePair<string, string>(s, s)).ToList(),
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

        public async Task UpdateAsync()
        {
            currencyItem.IsBusy = true;

            var theme = Settings.Theme.ToString("g");
            themeItem.SelectedItemIndex = themeItem.Items.FindIndex((t) => t.Key == theme);

            var sortOrder = Settings.SortOrder.ToString("g");
            sortByItem.SelectedItemIndex = sortByItem.Items.FindIndex((s) => s.Key == sortOrder);

            suspendSleepItem.IsSelected = DeviceDisplay.KeepScreenOn;

            var rates = await TokenInfoService.GetRatesAsync();
            await Cache.UpdateRatesAsync(rates);

            var symbols = rates.Select(t => new KeyValuePair<string, string>(t.Id, t.Symbol)).ToList();
            currencyItem.Items = symbols;

            var symbolIdx = symbols.FindIndex((s) => s.Key == Settings.CurrencyId);
            currencyItem.SelectedItemIndex = Math.Max(symbolIdx, 0);

            currencyItem.IsBusy = false;
        }

        #region Private

        private void OnCurrencyChanged(KeyValuePair<string, string> item)
        {
            Settings.CurrencyId = item.Key;
        }

        private void OnSortByChanged(KeyValuePair<string, string> item)
        {            
            Settings.SortOrder = (SortOrder)Enum.Parse(typeof(SortOrder), item.Key);
        }

        private void OnThemeChanged(KeyValuePair<string, string> item)
        {
            Settings.Theme = (Theme)Enum.Parse(typeof(Theme), item.Key);
        }

        private void OnSuspendSleepChanged(bool isSuspendSleepEnabled)
        {
            DeviceDisplay.KeepScreenOn = isSuspendSleepEnabled;            
        }

        #endregion
    }
}
