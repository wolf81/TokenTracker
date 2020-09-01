using System;
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

        private bool hasResults;
        public bool HasResults
        {
            get => hasResults;
            set => SetProperty(ref hasResults, value);
        }

        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set => SetProperty(ref errorMessage, value);
        }

        private LoadingState loadingState;
        public LoadingState LoadingState
        {
            get => loadingState;
            set => SetProperty(ref loadingState, value);
        }

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
            if (query.Length < 2)
            {
                Tokens = new ObservableCollection<Token> { };
                LoadingState = LoadingState.None;
                return;
            }
            
            IsBusy = true;

            LoadingState = LoadingState.Loading;

            try
            {
                var tokens = await TokenInfoService.GetTokensAsync(query);
                Tokens = new ObservableCollection<Token>(tokens.ToList());
                HasResults = Tokens.Count > 0;
                LoadingState = LoadingState.Finished;

                // TODO: show different views for 0 or 1+ results
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                ErrorMessage = ex.Message;
                LoadingState = LoadingState.Failed;
            }

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
