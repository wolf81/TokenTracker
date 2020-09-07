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
        private ObservableCollection<WalletItem> items = new ObservableCollection<WalletItem> {
            new WalletItem { Title = "BTC", Amount = 1, Price = new decimal(4556.45), Total = new decimal(4556.45) },
            new WalletItem { Title = "ETH", Amount = 3, Price = new decimal(1334.555535), Total = new decimal(4003.67) },
            new WalletItem { Title = "VET", Amount = 15, Price = new decimal(0.4234), Total = new decimal(6.351) },
            new WalletItem { Title = "OMG", Amount = 300, Price = new decimal(6.56), Total = new decimal(1968) },
        };
        public ObservableCollection<WalletItem> Items {
            get => items;
            set => SetProperty(ref items, value);
        }

        private DisplayMode displayMode = DisplayMode.View;
        public DisplayMode DisplayMode
        {
            get => displayMode;
            set => SetProperty(ref displayMode, value);
        }

        public TokenWalletViewModel()
        {
            Title = "Wallet";
        }

        #region Private

        #endregion
    }
}
