namespace TokenTracker.Services
{
    public interface ISettingsService
    {
        bool UseMocks { get; set; }

        bool IsFirstRun { get; set; }
    }
}
