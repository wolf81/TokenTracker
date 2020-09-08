using System;
using TokenTracker.Models;

namespace TokenTracker.Services
{
    public interface ISettingsService
    {
        event EventHandler<SortOrder> SortOrderChanged;

        bool UseMocks { get; set; }

        bool IsFirstRun { get; set; }

        SortOrder SortOrder { get; set; }
    }
}
