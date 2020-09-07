using System.Collections.ObjectModel;
using TokenTracker.Models;
using TokenTracker.ViewModels.Base;

namespace TokenTracker.ViewModels
{
    public class TokenWalletViewModel : ViewModelBase
    {
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
    }
}
