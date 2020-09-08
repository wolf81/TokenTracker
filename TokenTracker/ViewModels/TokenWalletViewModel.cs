using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TokenTracker.Models;
using TokenTracker.ViewModels.Base;

namespace TokenTracker.ViewModels
{
    public class TokenWalletViewModel : ViewModelBase
    {
        private readonly WalletAddTokenItem addItem = new WalletAddTokenItem { };

        private WalletViewTotalItem totalItem = new WalletViewTotalItem { };

        private ObservableCollection<WalletItemBase> items = new ObservableCollection<WalletItemBase> { };
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

            Items = new ObservableCollection<WalletItemBase>(new List<WalletItemBase> {
                new WalletViewTokenItem { Symbol = "BTC", Amount = 1, Price = new decimal(4556.45) },
                new WalletViewTokenItem { Symbol = "ETH", Amount = 3, Price = new decimal(1334.555535) },
                new WalletViewTokenItem { Symbol = "VET", Amount = 15, Price = new decimal(0.4234) },
                new WalletViewTokenItem { Symbol = "OMG", Amount = 300, Price = new decimal(6.56) },
            });

            Update();
        }

        #region Private

        private void Update()
        {
            if (DisplayMode == DisplayMode.Edit)
            {
                items.Remove(totalItem);
                items.Add(addItem);

                totalItem = null;
            }
            else
            {
                var totalPrice = Items.Sum((t) => t.Amount * t.Price);
                totalItem = new WalletViewTotalItem { Amount = 1, Price = totalPrice };

                items.Remove(addItem);
                items.Add(totalItem);
            }
        }

        #endregion
    }
}
