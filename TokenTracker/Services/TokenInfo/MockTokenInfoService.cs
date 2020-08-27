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
            new Token("btc", "BTC", "Bitcoin", new decimal(2356.5335), 0),
            new Token("bcc", "BCS", "Bitcoin Cash", new decimal(24.76445), 0),
            new Token("btg", "BTG", "Bitcoin Gold", new decimal(45.6666), 0),
            new Token("omg", "OMG", "Omise GO", new decimal(6.454666), 0),
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
