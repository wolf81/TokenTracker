using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using TokenTracker.Models;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker.ViewModels
{
    public class TokenWalletViewModel : ViewModelBase
    {
        public ICommand AddSymbolCommand => new Command(async () => await AddSymbolAsync());
        
        private ObservableCollection<WalletItem> items = new ObservableCollection<WalletItem> {
            new WalletItem { Title = "BTC", Amount = new decimal(4.556) },
            new WalletItem { Title = "ETH", Amount = new decimal(1334.555535) },
            new WalletItem { Title = "VET", Amount = new decimal(0.4234) },
            new WalletItem { Title = "OMG", Amount = new decimal(44455.56) },
        };
        public ObservableCollection<WalletItem> Items {
            get => items;
            set => SetProperty(ref items, value);
        }

        public TokenWalletViewModel()
        {
            Title = "Wallet";
        }

        #region Private

        private async Task AddSymbolAsync()
        {
            await NavigationService.NavigateToAsync<TokenSearchViewModel>();
        }

        #endregion
    }
}
