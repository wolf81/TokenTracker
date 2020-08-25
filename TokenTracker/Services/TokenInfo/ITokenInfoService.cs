using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenTracker.Models;

namespace TokenTracker.Services
{
    public interface ITokenInfoService
    {
        event EventHandler<Dictionary<string, decimal>> TokensUpdated;

        void StartTokenUpdates();

        void StopTokenUpdates();

        Task<IEnumerable<Token>> GetTokensAsync();

        Task<IEnumerable<Token>> GetTokensAsync(string query);
    }
}
