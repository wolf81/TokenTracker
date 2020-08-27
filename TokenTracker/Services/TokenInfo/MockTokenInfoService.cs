using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenTracker.Models;

namespace TokenTracker.Services
{
    public class MockTokenInfoService : ITokenInfoService
    {
        public event EventHandler<Dictionary<string, decimal>> TokensUpdated;

        public event EventHandler<ConnectionState> ConnectionStateChanged;

        public void StartTokenUpdates()
        {
            throw new NotImplementedException();
        }

        public void StopTokenUpdates()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Token>> GetTokensAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Token>> GetTokensAsync(string tokenIdOrSymbol)
        {
            throw new NotImplementedException();
        }
    }
}
