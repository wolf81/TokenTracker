using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker.ViewModels
{
    public class TokenWalletViewModel : ViewModelBase
    {
        private ITokenCache TokenCache => ViewModelLocator.Resolve<ITokenCache>();

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

            var currencySymbol = ViewModelLocator.Resolve<ISettingsService>().Currency;

            Items = new ObservableCollection<WalletItemBase>(new List<WalletItemBase> {
                new WalletViewTokenItem { TokenSymbol = "BTC", Amount = 1, Price = new decimal(4556.45), CurrencySymbol = currencySymbol },
                new WalletViewTokenItem { TokenSymbol = "ETH", Amount = 3, Price = new decimal(1334.555535), CurrencySymbol = currencySymbol },
                new WalletViewTokenItem { TokenSymbol = "VET", Amount = 15, Price = new decimal(0.4234), CurrencySymbol = currencySymbol },
                new WalletViewTokenItem { TokenSymbol = "OMG", Amount = 300, Price = new decimal(6.56), CurrencySymbol = currencySymbol },
            });

            totalItem.CurrencySymbol = currencySymbol;

            TokenCache.TokenUpdated += Handle_TokenCache_TokenUpdated;

            Update();
        }

        #region Private

        private void Handle_TokenCache_TokenUpdated(object sender, Token token)
        {
            if (DisplayMode == DisplayMode.Edit) { return; }

            // enumerate over a copy, to prevent crashes if the collection would change
            if (Items.ToList().FirstOrDefault((t) => t.TokenSymbol == token.Symbol) is WalletItemBase walletItem)
            {
                var tokenIdx = Items.IndexOf(walletItem);
                walletItem.Price = token.PriceUSD;
                Device.BeginInvokeOnMainThread(() => Items[tokenIdx] = walletItem);
            }

            decimal totalPrice = 0;
            foreach (var item in Items.ToList())
            {
                if (item is WalletViewTokenItem tokenItem)
                {
                    totalPrice += tokenItem.TotalPrice;
                }
            }

            var currencySymbol = ViewModelLocator.Resolve<ISettingsService>().Currency;
            var totalItem = new WalletViewTotalItem { Amount = 1, Price = totalPrice, CurrencySymbol = currencySymbol };
            Device.BeginInvokeOnMainThread(() => Items[Items.Count - 1] = totalItem);
        }

        private void Update()
        {
            var currencySymbol = ViewModelLocator.Resolve<ISettingsService>().Currency;

            if (DisplayMode == DisplayMode.Edit)
            {
                var lastIdx = Items.Count - 1;
                if (Items[lastIdx] is WalletViewTotalItem)
                {
                    Items.RemoveAt(lastIdx);
                }

                if (items.Contains(addItem) == false)
                {
                    items.Add(addItem);
                }
            }
            else
            {
                if (items.Contains(addItem))
                {
                    items.Remove(addItem);
                }
                
                var totalPrice = Items.Sum((t) => t.Amount * t.Price);
                totalItem = new WalletViewTotalItem { Amount = 1, Price = totalPrice, CurrencySymbol = currencySymbol };

                var lastIdx = Items.Count - 1;
                if (Items[lastIdx] is WalletViewTotalItem)
                {
                    Items[lastIdx] = totalItem;
                }
                else
                {
                    Items.Add(totalItem);
                }
            }
        }

        #endregion
    }
}
