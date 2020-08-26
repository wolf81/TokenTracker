using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker.ViewModels
{
    public class TokenListViewModel : ViewModelBase
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        public ICommand ReloadCommand => new Command(async () => await GetTokenInfoAsync());

        public ICommand EditCommand => new Command(Edit);

        private ObservableCollection<Token> tokens;
        public ObservableCollection<Token> Tokens
        {
            get => tokens;
            set => SetProperty(ref tokens, value);
        }

        public TokenListViewModel()
        {
            ReloadCommand.Execute(null);

            Title = "Token Tracker";

            TokenInfoService.StartTokenUpdates();
            TokenInfoService.TokensUpdated += Handle_TokenInfoService_TokensUpdated;
        }

        #region Private

        private void Edit(object parameter)
        {
            if (Tokens.Contains(Token.AddToken) == false)
            {
                Tokens.Add(Token.AddToken);
            }
            else
            {
                Tokens.Remove(Token.AddToken);
            }
        }

        private async Task GetTokenInfoAsync()
        {
            var tokens = await TokenInfoService.GetTokensAsync("bitcoin");
            Tokens = new ObservableCollection<Token>(tokens);
        }

        private void Handle_TokenInfoService_TokensUpdated(object sender, Dictionary<string, decimal> tokenPriceInfo)
        {
            foreach (var kv in tokenPriceInfo)
            {
                var token = Tokens.FirstOrDefault((t) => t.Id == kv.Key);
                var tokenIdx = Tokens.IndexOf(token);
                
                if (token != null)
                {
                    var newToken = (Token) token.Clone();
                    newToken.PriceUSD = kv.Value;

                    Device.BeginInvokeOnMainThread(() => {
                        Tokens[tokenIdx] = newToken;
                    });
                }
            }
        }

        #endregion
    }
}
