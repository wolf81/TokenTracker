using System;
using TokenTracker.Models;

namespace TokenTracker.Services
{
    public interface ISettingsService
    {
        event EventHandler<SortOrder> SortOrderChanged;

        event EventHandler<string> CurrencyIdChanged;

        bool UseMocks { get; set; }

        bool IsFirstRun { get; set; }

        string CurrencyId { get; set; }

        SortOrder SortOrder { get; set; }
    }
}
