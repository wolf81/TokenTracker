using System.Collections.Generic;
using System.Threading.Tasks;
using TokenTracker.Models;

namespace TokenTracker.Services.TokenCache
{
    public interface ITokenCache
    {
        Task AddTokenAsync(Token token);

        Task RemoveTokenAsync(Token token);

        Task<IEnumerable<Token>> GetTokensAsync();
    }
}
