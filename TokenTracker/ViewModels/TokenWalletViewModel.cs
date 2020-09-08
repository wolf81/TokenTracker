using System.Collections.ObjectModel;
using TokenTracker.Models;
using TokenTracker.ViewModels.Base;

namespace TokenTracker.ViewModels
{
    public class TokenWalletViewModel : ViewModelBase
    {
        private readonly WalletAddTokenItem addItem = new WalletAddTokenItem { };

        private readonly WalletViewTotalItem totalItem = new WalletViewTotalItem { };

        private ObservableCollection<WalletItemBase> items = new ObservableCollection<WalletItemBase> {
            new WalletViewTokenItem { Symbol = "BTC", Amount = 1, Price = new decimal(4556.45) },
            new WalletViewTokenItem { Symbol = "ETH", Amount = 3, Price = new decimal(1334.555535) },
            new WalletViewTokenItem { Symbol = "VET", Amount = 15, Price = new decimal(0.4234) },
            new WalletViewTokenItem { Symbol = "OMG", Amount = 300, Price = new decimal(6.56) },
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
                items.Remove(totalItem);
                items.Add(addItem);
            }
            else
            {
                items.Remove(addItem);
                items.Add(totalItem);
            }
        }

        #endregion
    }
}
