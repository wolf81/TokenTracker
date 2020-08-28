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
    public class TokenSearchViewModel : ViewModelBase
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        private ITokenCache TokenCache => ViewModelLocator.Resolve<ITokenCache>();

        public ICommand SearchTokenCommand => new Command<string>(async (q) => await SearchTokenAsync(q));

        public ICommand AddTokenCommand => new Command<Token>(async (t) => await AddTokenAsync(t));

        private ObservableCollection<Token> tokens;
        public ObservableCollection<Token> Tokens
        {
            get => tokens;
            set => SetProperty(ref tokens, value);
        }

        public TokenSearchViewModel()
        {
            Title = "Search";
        }

        public async Task SearchTokenAsync(string query)
        {
            IsBusy = true;
            var tokens = await TokenInfoService.GetTokensAsync(query);
            Tokens = new ObservableCollection<Token>(tokens.ToList());
            await Task.Delay(2);
            IsBusy = false;
        }

        #region Private
    
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
