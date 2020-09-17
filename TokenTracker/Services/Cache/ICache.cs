using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TokenTracker.Models;

namespace TokenTracker.Services
{
    public interface ICache
    {
        event EventHandler<Token> TokenAdded;

        event EventHandler<Token> TokenRemoved;

        event EventHandler<Token> TokenUpdated;

        Task AddTokenAsync(Token token);

        Task RemoveTokenAsync(Token token);

        Task UpdateTokenAsync(Token token);

        Task<IEnumerable<Token>> GetTokensAsync(SortOrder sortOrder);

        Task<Token> GetTokenAsync(string id);

        Task UpdateRatesAsync(IEnumerable<Rate> tates);

        Task<Rate> GetRateAsync(string id);        

        void Configure();

        Task ResetAsync();
    }
}
