using System.Collections.Generic;

namespace TokenTracker.Services
{
    public interface ISettingsService
    {
        bool UseMocks { get; set; }

        List<string> TrackedTokens { get; set; }
    }
}
