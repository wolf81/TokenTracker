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
    public class TokenStatusViewModel : ViewModelBase
    {
        private ITokenInfoService TokenInfoService => ViewModelLocator.Resolve<ITokenInfoService>();

        private IMessageService MessageService => DependencyService.Get<IMessageService>();

        private ITokenCache TokenCache => ViewModelLocator.Resolve<ITokenCache>();

        public ICommand ReloadCommand => new Command(async () => await GetTokenInfoAsync());

        public ICommand RemoveTokenCommand => new Command<Token>(async (t) => await RemoveTokenAsync(t));

        public ICommand AddTokenCommand => new Command(async (t) => await AddTokenAsync());

        public ICommand ChangeIntervalCommand => new Command<Interval>(async (i) => await ChangeTokenIntervalAsync(i));

        public ICommand HideChartCommand => new Command(HideChart);

        private Token selectedToken;
        public Token SelectedToken
        {
            get => selectedToken;
            set => SetProperty(ref selectedToken, value);
        }

        private IEnumerable<PricePoint> tokenPriceHistory;
        public IEnumerable<PricePoint> TokenPriceHistory
        {
            get => tokenPriceHistory;
            set => SetProperty(ref tokenPriceHistory, value);
        }

        private bool showChart = false;
        public bool ShowChart
        {
            get => showChart;
            set => SetProperty(ref showChart, value);
        }

        private DisplayMode displayMode = DisplayMode.View;
        public DisplayMode DisplayMode
        {
            get => displayMode;
            set => SetProperty(ref displayMode, value);
        }

        private ObservableCollection<Token> tokens = new ObservableCollection<Token> { };
        public ObservableCollection<Token> Tokens
        {
            get => tokens;
            set => SetProperty(ref tokens, value);
        }

        public TokenStatusViewModel()
        {
            ReloadCommand.Execute(null);

            Title = "Live";

            TokenInfoService.TokensUpdated += Handle_TokenInfoService_TokensUpdated;
            TokenCache.TokenAdded += Handle_TokenCache_TokenAdded;
            TokenCache.TokenRemoved += Handle_TokenCache_TokenRemoved;
            TokenCache.TokenUpdated += Handle_TokenCache_TokenUpdated;
        }

        public async Task RefreshTokensAsync()
        {
            var tokens = await TokenCache.GetTokensAsync();
            Tokens = new ObservableCollection<Token>(tokens);
        }

        #region Private

        private void HideChart()
        {
            SelectedToken = null;
            ShowChart = false;
        }

        private async Task GetTokenInfoAsync()
        {            
            var tokens = await TokenCache.GetTokensAsync();
            Tokens = new ObservableCollection<Token>(tokens);
        }

        private async Task AddTokenAsync()
        {
            if (DisplayMode == DisplayMode.View) { return; }

            await NavigationService.NavigateToAsync<TokenSearchViewModel>();
        }

        private async Task ChangeTokenIntervalAsync(Interval interval)
        {
            try
            {
                TokenPriceHistory = await TokenInfoService.GetTokenHistoryAsync("bitcoin", interval);
                Console.WriteLine(TokenPriceHistory);
            }
            catch (Exception ex)
            {
                MessageService.Show(ex.Message, DisplayDuration.Long);
            }
        }

        private async Task RemoveTokenAsync(Token token)
        {
            switch (DisplayMode)
            {
                case DisplayMode.View:
                    SelectedToken = token;
                    ShowChart = !ShowChart;
                    break;
                case DisplayMode.Edit:
                    await TokenCache.RemoveTokenAsync(token);
                    break;
            }
        }

        private async void Handle_TokenInfoService_TokensUpdated(object sender, Dictionary<string, decimal> tokenPriceInfo)
        {
            if (ShowChart == true) { return; }

            foreach (var kv in tokenPriceInfo)
            {
                if (await TokenCache.GetTokenAsync(kv.Key) is Token token)
                {
                    token.PriceUSD = kv.Value;
                    await TokenCache.UpdateTokenAsync(token);
                }
            }
        }

        private void Handle_TokenCache_TokenRemoved(object sender, Token token)
        {
            // enumerate over a copy, to prevent crashes if the collection would change
            if (Tokens.ToList().FirstOrDefault((t) => t.Id == token.Id) is Token matchedToken)
            {
                Tokens.Remove(matchedToken);
                MessageService.Show($"Remove {token.Symbol}", DisplayDuration.Short);
            }
        }

        private void Handle_TokenCache_TokenAdded(object sender, Token token)
        {
            // enumerate over a copy, to prevent crashes if the collection would change
            if (Tokens.ToList().FirstOrDefault((t) => t.Id == token.Id) == null)
            {
                Tokens.Add(token);
                MessageService.Show($"Add {token.Symbol}", DisplayDuration.Short);
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
