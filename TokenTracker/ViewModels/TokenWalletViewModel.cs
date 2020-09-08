using System.Collections.ObjectModel;
using TokenTracker.Models;
using TokenTracker.ViewModels.Base;

namespace TokenTracker.ViewModels
{
    public class TokenWalletViewModel : ViewModelBase
    {
        private WalletAddItem addItem = new WalletAddItem { };

        private ObservableCollection<WalletItemBase> items = new ObservableCollection<WalletItemBase> {
            new WalletViewItem { Symbol = "BTC", Amount = 1, Price = new decimal(4556.45) },
            new WalletViewItem { Symbol = "ETH", Amount = 3, Price = new decimal(1334.555535) },
            new WalletViewItem { Symbol = "VET", Amount = 15, Price = new decimal(0.4234) },
            new WalletViewItem { Symbol = "OMG", Amount = 300, Price = new decimal(6.56) },
        };
        public ObservableCollection<WalletItemBase> Items {
            get => items;
            set => SetProperty(ref items, value);
        }

        private DisplayMode displayMode = DisplayMode.View;
        public DisplayMode DisplayMode
        {
            get => displayMode;
            set
            {
                SetProperty(ref displayMode, value);
                Update();
            }
        }

        public TokenWalletViewModel()
        {
            Title = "Wallet";
        }

        #region Private

        private void Update()
        {
            if (DisplayMode == DisplayMode.Edit)
            {
                items.Add(addItem);
            }
            else
            {
                items.Remove(addItem);
            }
        }

        #endregion
    }
}
