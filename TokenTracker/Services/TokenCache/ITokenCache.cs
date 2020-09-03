using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenTracker.Models;

namespace TokenTracker.Services
{
    public interface ITokenCache
    {
        event EventHandler<Token> TokenAdded;

        event EventHandler<Token> TokenRemoved;

        event EventHandler<Token> TokenUpdated;

        Task AddTokenAsync(Token token);

        Task RemoveTokenAsync(Token token);

        Task UpdateTokenAsync(Token token);

        Task<IEnumerable<Token>> GetTokensAsync();

        Task<Token> GetTokenAsync(string id);

        Task AddRatesAsync(IEnumerable<Rate> tates);

        Task<Rate> GetRateAsync(string id);

        void Configure();
    }
}
