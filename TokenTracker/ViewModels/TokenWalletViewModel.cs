﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using TokenTracker.Models;
using TokenTracker.Services;
using TokenTracker.ViewModels.Base;
using Xamarin.Forms;

namespace TokenTracker.ViewModels
{
    public class TokenWalletViewModel : ViewModelBase, IDisposable
    {
        private ICache Cache => ViewModelLocator.Resolve<ICache>();

        private ISettingsService SettingsService => ViewModelLocator.Resolve<ISettingsService>();

        private decimal multiplyFactor = new decimal(1.0);

        private readonly WalletAddTokenItem addItem = new WalletAddTokenItem { };

        private WalletViewTotalItem totalItem = new WalletViewTotalItem { };

        private ObservableCollection<WalletItemBase> items = new ObservableCollection<WalletItemBase> { };
        public ObservableCollection<WalletItemBase> Items
        {
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
                Task.Run(async () => await UpdateAsync());                
            }
        }

        public TokenWalletViewModel()
        {
            Title = "Wallet";

            Items = new ObservableCollection<WalletItemBase>(new List<WalletItemBase> {
                new WalletViewTokenItem { TokenSymbol = "BTC", Amount = 1, Price = new decimal(4556.45) },
                new WalletViewTokenItem { TokenSymbol = "ETH", Amount = 3, Price = new decimal(1334.555535) },
                new WalletViewTokenItem { TokenSymbol = "VET", Amount = 15, Price = new decimal(0.4234) },
                new WalletViewTokenItem { TokenSymbol = "OMG", Amount = 300, Price = new decimal(6.56) },
            });

            SettingsService.CurrencyIdChanged += Handle_SettingsService_CurrencyIdChanged;
            Cache.TokenUpdated += Handle_Cache_TokenUpdated;

            Task.Run(async () => await UpdateAsync());
        }

        public void Dispose()
        {
            SettingsService.CurrencyIdChanged -= Handle_SettingsService_CurrencyIdChanged;
            Cache.TokenUpdated -= Handle_Cache_TokenUpdated;
        }

        #region Private

        private async void Handle_SettingsService_CurrencyIdChanged(object sender, string currencyId)
        {
            var rate = await Cache.GetRateAsync(currencyId);
            multiplyFactor = decimal.Divide(1, rate.RateUSD);

            var totalPrice = new decimal(0);
            foreach (var item in Items)
            {
                item.CurrencySymbol = rate.Symbol;
                item.MultiplyFactor = multiplyFactor;

                if (item is WalletViewTokenItem tokenItem)
                {
                    totalPrice += tokenItem.TotalPrice;
                }
            }

            var lastIdx = Items.Count - 1;
            if (Items[lastIdx] is WalletViewTotalItem)
            {
                var totalItem = new WalletViewTotalItem { Amount = 1, Price = totalPrice, CurrencySymbol = rate.Symbol };
                Device.BeginInvokeOnMainThread(() => Items[lastIdx] = totalItem);
            }
        }

        private async void Handle_Cache_TokenUpdated(object sender, Token token)
        {
            if (DisplayMode == DisplayMode.Edit) { return; }

            // enumerate over a copy, to prevent crashes if the collection would change
            if (Items.ToList().FirstOrDefault((t) => t.TokenSymbol == token.Symbol) is WalletItemBase walletItem)
            {
                var tokenIdx = Items.IndexOf(walletItem);
                walletItem.Price = token.PriceUSD;
                Device.BeginInvokeOnMainThread(() => Items[tokenIdx] = walletItem);
            }

            var totalPrice = new decimal(0);
            foreach (var item in Items.ToList())
            {
                if (item is WalletViewTokenItem tokenItem)
                {
                    totalPrice += tokenItem.TotalPrice;
                }
            }

            var rate = await Cache.GetRateAsync(SettingsService.CurrencyId) ?? Rate.Default;
            var totalItem = new WalletViewTotalItem { Amount = 1, Price = totalPrice, CurrencySymbol = rate.Symbol};
            Device.BeginInvokeOnMainThread(() => Items[Items.Count - 1] = totalItem);
        }

        private async Task UpdateAsync()
        {
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

                var rate = await Cache.GetRateAsync(SettingsService.CurrencyId);
                foreach (var item in Items)
                {
                    item.CurrencySymbol = rate.Symbol;
                }

                var totalPrice = Items.Sum((t) => t.Amount * t.Price);
                totalItem = new WalletViewTotalItem { Amount = 1, Price = totalPrice, CurrencySymbol = rate.Symbol };

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
