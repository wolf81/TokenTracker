using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TokenTracker.Models;
using Xamarin.Forms;

namespace TokenTracker.Services
{
    public class MockTokenInfoService : ITokenInfoService
    {
        public ConnectionState State { get; private set; } = ConnectionState.Disconnected;

        public event EventHandler<Dictionary<string, decimal>> TokensUpdated;

        public event EventHandler<ConnectionState> ConnectionStateChanged;

        private readonly List<Token> tokens = new List<Token>
        {
            new Token { Id = "btc", Symbol = "BTC", Name = "Bitcoin", PriceUSD = new decimal(2356.5335435), Change24 = new decimal(3.56) },
            new Token { Id = "bch", Symbol = "BCH", Name = "Bitcoin Cash", PriceUSD = new decimal(123.554453453), Change24 = new decimal(1.55) },
            new Token { Id = "bcd", Symbol = "BCD", Name = "Bitcoin Diamond", PriceUSD = new decimal(434.55334543), Change24 = new decimal(-1.25) },
            new Token { Id = "omg", Symbol = "OMG", Name = "Omise GO", PriceUSD = new decimal(6.5453), Change24 = new decimal(-2.44) },
            new Token { Id = "eth", Symbol = "ETH", Name = "Ethereum", PriceUSD = new decimal(442.554345), Change24 = new decimal(4.44) },
            new Token { Id = "etc", Symbol = "ETC", Name = "Ethereum Classic", PriceUSD = new decimal(321.3344), Change24 = new decimal(2.24) },
            new Token { Id = "vet", Symbol = "VET", Name = "VeChain", PriceUSD = new decimal(44.3213), Change24 = new decimal(0.53) },
            new Token { Id = "dog", Symbol = "DOGE", Name = "Dogecoin", PriceUSD = new decimal(33.3133), Change24 = new decimal(-0.56) },
        };

        public MockTokenInfoService()
        {
            Task.Run(UpdateTokenPricesContinuously);
        }

        public void StartTokenUpdates()
        {
            OnConnectionStateChanged(ConnectionState.Busy);

            Task.Delay(1000).ContinueWith((t) => OnConnectionStateChanged(ConnectionState.Connected));
        }

        public void StopTokenUpdates()
        {
            OnConnectionStateChanged(ConnectionState.Busy);

            Task.Delay(1000).ContinueWith((t) => OnConnectionStateChanged(ConnectionState.Disconnected));
        }

        public async Task<IEnumerable<Token>> GetTokensAsync()
        {
            await Task.Delay(1000);

            return tokens;
        }

        public async Task<IEnumerable<Token>> GetTokensAsync(string tokenIdOrSymbol)
        {
            await Task.Delay(1000);

            return tokens.FindAll((t) =>
            {
                var query = tokenIdOrSymbol.ToLower();
                var symbol = t.Symbol.ToLower();
                var name = t.Name.ToLower();
                return symbol.Contains(query) || name.Contains(query);
            });
        }

        #region Private

        private async void UpdateTokenPricesContinuously()
        {
            var random = new Random(DateTime.Now.Second);

            while (true)
            {
                var delay = random.Next(250, 1750);
                await Task.Delay(delay);

                var updateCount = Math.Max(tokens.Count() / 4, 4);
                updateCount = random.Next(updateCount);

                var tokenPriceInfo = new Dictionary<string, decimal> { };

                for (var i = 0; i < updateCount; i++)
                {
                    var updateIdx = random.Next(tokens.Count());
                    var change = (100 + random.Next(-6, 6)) / 100.0;

                    var id = tokens[updateIdx].Id;
                    var price = (double)tokens[updateIdx].PriceUSD;
                    var newPrice = new decimal(price * change);
                    tokens[updateIdx].PriceUSD = newPrice;

                    tokenPriceInfo[id] = newPrice;
                }

                if (State == ConnectionState.Connected)
                {
                    OnTokensUpdated(tokenPriceInfo);
                }
            }
        }

        private void OnTokensUpdated(Dictionary<string, decimal> tokenPriceInfo)
        {
            TokensUpdated?.Invoke(this, tokenPriceInfo);
        }

        private void OnConnectionStateChanged(ConnectionState state)
        {
            State = state;

            ConnectionStateChanged?.Invoke(this, State);
        }

        #endregion
    }
}
