using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TokenTracker.Services
{
    public class SettingsService : ISettingsService
    {
        private struct Keys
        {
            public const string UseMocks = "use_mocks";
            public const string IsFirstRun = "is_first_run";
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

        private Task AddOrUpdateValue(string key, bool value) => AddOrUpdateValueInternal(key, value);
        private bool GetValueOrDefault(string key, bool defaultValue) => GetValueOrDefaultInternal(key, defaultValue);

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
