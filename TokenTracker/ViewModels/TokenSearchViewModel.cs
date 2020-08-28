using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker.ViewModels
{
    public class TokenSearchViewModel : ViewModelBase
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        private ITokenCache TokenCache => ViewModelLocator.Resolve<ITokenCache>();

        public ICommand SearchTokenCommand => new Command<string>(async (q) => await SearchTokenAsync(q));

        public ICommand AddTokenCommand => new Command<Token>(async (t) => await AddTokenAsync(t));

        private List<Token> tokens;
        public List<Token> Tokens {
            get => tokens;
            set => SetProperty(ref tokens, value);
        }

        public TokenSearchViewModel()
        {
            Title = "Search";
        }

        #region Private

        private async Task SearchTokenAsync(string query)
        {
            if (query.Length > 1)
            {
                IsBusy = true;
                var tokens = await TokenInfoService.GetTokensAsync(query);
                Tokens = tokens.ToList();
                IsBusy = false;
            }
            else
            {
                Tokens.Clear();
            }
        }

        private async Task AddTokenAsync(Token token)
        {
            if (await TokenCache.GetTokenAsync(token.Id) == null)
            {
                await TokenCache.AddTokenAsync(token);
            }
            else
            {
                await TokenCache.RemoveTokenAsync(token);
            }
        }

        #endregion
    }
}
