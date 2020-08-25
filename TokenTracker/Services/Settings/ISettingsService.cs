using System.Threading.Tasks;

namespace TokenTracker.Services
{
    public interface ISettingsService
    {
        bool UseMocks { get; set; }

        bool GetValueOrDefault(string key, bool defaultValue);

        string GetValueOrDefault(string key, string defaultValue);

        Task AddOrUpdateValue(string key, bool value);

        Task AddOrUpdateValue(string key, string value);
    }
}
