using System;
using System.Threading.Tasks;
using TokenTracker.Models;
using Xamarin.Forms;

namespace TokenTracker.Services
{
    public class SettingsService : ISettingsService
    {
        public event EventHandler<SortOrder> SortOrderChanged;

        public event EventHandler<string> CurrencyIdChanged;

        private struct Keys
        {
            public const string UseMocks = "use_mocks";
            public const string IsFirstRun = "is_first_run";
            public const string SortOrder = "sort_order";
            public const string CurrencyId = "currency_id";
        }

        public bool UseMocks
        {
            get => GetValueOrDefault(Keys.UseMocks, false);
            set => AddOrUpdateValue(Keys.UseMocks, value);
        }

        public bool IsFirstRun
        {
            get => GetValueOrDefault(Keys.IsFirstRun, true);
            set => AddOrUpdateValue(Keys.IsFirstRun, value);
        }

        public string CurrencyId
        {
            get => GetValueOrDefault(Keys.CurrencyId, Rate.DEFAULT_RATE_ID);
            set
            {
                AddOrUpdateValue(Keys.CurrencyId, value);
                OnCurrencyIdChanged(value);
            }
        }

        public SortOrder SortOrder
        {
            get => (SortOrder)GetValueOrDefault(Keys.SortOrder, (int)SortOrder.Rank);
            set
            {
                AddOrUpdateValue(Keys.SortOrder, (int)value);
                OnSortOrderChanged(value);
            }
        }

        #region Private

        private Task AddOrUpdateValue(string key, string value) => AddOrUpdateValueInternal(key, value);
        private Task AddOrUpdateValue(string key, bool value) => AddOrUpdateValueInternal(key, value);
        private Task AddOrUpdateValue(string key, int value) => AddOrUpdateValueInternal(key, value);
        private string GetValueOrDefault(string key, string defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        private bool GetValueOrDefault(string key, bool defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        private int GetValueOrDefault(string key, int defaultValue) => GetValueOrDefaultInternal(key, defaultValue);

        private async Task AddOrUpdateValueInternal<T>(string key, T value)
        {
            if (value == null)
            {
                await Remove(key);
            }

            Application.Current.Properties[key] = value;
            try
            {
                await Application.Current.SavePropertiesAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to save: " + key, " Message: " + ex.Message);
            }
        }

        private T GetValueOrDefaultInternal<T>(string key, T defaultValue = default)
        {
            object value = null;
            if (Application.Current.Properties.ContainsKey(key))
            {
                value = Application.Current.Properties[key];
            }
            return null != value ? (T)value : defaultValue;
        }

        private async Task Remove(string key)
        {
            try
            {
                if (Application.Current.Properties[key] != null)
                {
                    Application.Current.Properties.Remove(key);
                    await Application.Current.SavePropertiesAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Unable to remove: " + key, " Message: " + ex.Message);
            }
        }

        private void OnSortOrderChanged(SortOrder sortOrder)
        {
            SortOrderChanged?.Invoke(this, sortOrder);
        }

        private void OnCurrencyIdChanged(string currencySymbol)
        {
            CurrencyIdChanged?.Invoke(this, currencySymbol);
        }

        #endregion
    }
}
