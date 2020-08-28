using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.Services.TokenCache;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker.ViewModels
{
    public class TokenListViewModel : ViewModelBase
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        private ITokenCache TokenCache => ViewModelLocator.Resolve<ITokenCache>();

        public ICommand ReloadCommand => new Command(async () => await GetTokenInfoAsync());

        public ICommand TokenActionCommand => new Command(async (p) => await PerformTokenActionAsync(p));

        private ObservableCollection<Token> tokens = new ObservableCollection<Token> { };
        public ObservableCollection<Token> Tokens
        {
            get => tokens;
            set => SetProperty(ref tokens, value);
        }

        public TokenListViewModel()
        {
            ReloadCommand.Execute(null);

            Title = "Token Tracker";

            TokenInfoService.TokensUpdated += Handle_TokenInfoService_TokensUpdated;
        }

        #region Private

        private async Task GetTokenInfoAsync()
        {            
            var tokens = await TokenCache.GetTokensAsync();
            Tokens = new ObservableCollection<Token>(tokens);
        }

        private async Task PerformTokenActionAsync(object parameter)
        { 
            if (parameter is Token token)
            {
                if (token == Token.Dummy)
                {
                    await NavigationService.NavigateToAsync<TokenSearchViewModel>();
                }
            }
        }

        private void Handle_TokenInfoService_TokensUpdated(object sender, Dictionary<string, decimal> tokenPriceInfo)
        {
            foreach (var kv in tokenPriceInfo)
            {
                // create a copy to prevent issue: "Collection was modified; enumeration operation may not execute."
                var tokens = Tokens.ToList();
                if (tokens.FirstOrDefault((t) => t.Id == kv.Key) is Token token)
                {
                    token.PriceUSD = kv.Value;

                    var tokenIdx = Tokens.IndexOf(token);
                    Device.BeginInvokeOnMainThread(() => {
                        Tokens[tokenIdx] = token;
                    });
                }
            }
        }

        #endregion
    }
}
