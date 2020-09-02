using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenTracker.Models;

namespace TokenTracker.Services
{
    public enum ConnectionState { Busy, Connected, Disconnected }

    public enum Interval { Day1, Week1, Month1, Year1 }

    public interface ITokenInfoService
    {
        event EventHandler<Dictionary<string, decimal>> TokensUpdated;

        event EventHandler<ConnectionState> ConnectionStateChanged;

        void Configure(IEnumerable<string> tokenIds);

        void StartTokenUpdates();

        void StopTokenUpdates();

        Task<IEnumerable<Token>> GetTokensAsync();

        Task<IEnumerable<Token>> GetTokensAsync(string query);

        Task<IEnumerable<PricePoint>> GetTokenHistoryAsync(string tokenId, Interval interval);
    }
}
