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
            TokenCache.TokenAdded += Handle_TokenCache_TokenAdded;
            TokenCache.TokenRemoved += Handle_TokenCache_TokenRemoved;
            TokenCache.TokenUpdated += Handle_TokenCache_TokenUpdated;
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

        private async void Handle_TokenInfoService_TokensUpdated(object sender, Dictionary<string, decimal> tokenPriceInfo)
        {
            foreach (var kv in tokenPriceInfo)
            {
                var token = await TokenCache.GetTokenAsync(kv.Key);
                token.PriceUSD = kv.Value;
                await TokenCache.UpdateTokenAsync(token);
            }
        }

        private void Handle_TokenCache_TokenRemoved(object sender, Token token)
        {
            // enumerate over a copy, to prevent crashes if the collection would change
            if (Tokens.ToList().FirstOrDefault((t) => t.Id == token.Id) is Token matchedToken)
            {
                Tokens.Remove(matchedToken);
            }
        }

        private void Handle_TokenCache_TokenAdded(object sender, Token token)
        {
            // enumerate over a copy, to prevent crashes if the collection would change
            if (Tokens.ToList().FirstOrDefault((t) => t.Id == token.Id) == null)
            {
                var dummyTokenIdx = Tokens.IndexOf(Token.Dummy);
                Tokens.Insert(dummyTokenIdx, token);
            }
        }

        private void Handle_TokenCache_TokenUpdated(object sender, Token token)
        {
            // enumerate over a copy, to prevent crashes if the collection would change
            if (Tokens.ToList().FirstOrDefault((t) => t.Id == token.Id) is Token matchedToken)
            {
                var tokenIdx = Tokens.IndexOf(matchedToken);
                Device.BeginInvokeOnMainThread(() => Tokens[tokenIdx] = token);
            }
        }

        #endregion
    }
}
