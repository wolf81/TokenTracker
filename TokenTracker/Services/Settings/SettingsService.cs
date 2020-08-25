using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TokenTracker.Services
{
    public class SettingsService : ISettingsService
    {
        #region Setting Constants

        private const string IdUseMocks = "use_mocks";
        private const string IdTrackedTokens = "tracked_tokens";

        private readonly bool UseMocksDefault = true;
        private readonly List<string> TrackedTokensDefault = new List<string> { "BTC", "ETH", "XRP" };

        #endregion

        public bool UseMocks
        {
            get => GetValueOrDefault(IdUseMocks, UseMocksDefault);
            set => AddOrUpdateValue(IdUseMocks, value);
        }

        public List<string> TrackedTokens {
            get => GetValueOrDefault(IdTrackedTokens, TrackedTokensDefault);
            set => AddOrUpdateValue(IdTrackedTokens, value);
        }

        private Task AddOrUpdateValue(string key, bool value) => AddOrUpdateValueInternal(key, value);
        private Task AddOrUpdateValue(string key, List<string> value) => AddOrUpdateValueInternal(key, value);
        private bool GetValueOrDefault(string key, bool defaultValue) => GetValueOrDefaultInternal(key, defaultValue);
        private List<string> GetValueOrDefault(string key, List<string> defaultValue) => GetValueOrDefaultInternal(key, defaultValue);

        #region Internal Implementation

        async Task AddOrUpdateValueInternal<T>(string key, T value)
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

        T GetValueOrDefaultInternal<T>(string key, T defaultValue = default)
        {
            object value = null;
            if (Application.Current.Properties.ContainsKey(key))
            {
                value = Application.Current.Properties[key];
            }
            return null != value ? (T)value : defaultValue;
        }

        async Task Remove(string key)
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

        #endregion
    }
}
