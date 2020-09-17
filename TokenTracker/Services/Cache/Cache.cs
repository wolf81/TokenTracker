using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using TokenTracker.Extensions;
using TokenTracker.Models;

namespace TokenTracker.Services
{
    public class Cache : ICache
    {
        public event EventHandler<Token> TokenAdded;

        public event EventHandler<Token> TokenRemoved;

        public event EventHandler<Token> TokenUpdated;

        private readonly SQLiteAsyncConnection database;

        private static readonly string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cache.db");

        public Cache()
        {
            Console.WriteLine($"Cache path: {filepath}");

            database = new SQLiteAsyncConnection(filepath);
            database.CreateTablesAsync<Token, Rate>().Wait();
        }

        public async Task AddTokenAsync(Token token)
        {
            token.PriceUSD = (decimal)NormalizedPrice((double)token.PriceUSD, 2);

            await database.InsertOrReplaceAsync(token);

            OnTokenAdded(token);
        }

        public async Task RemoveTokenAsync(Token token)
        {
            await database.DeleteAsync(token);

            OnTokenRemoved(token);
        }

        public async Task UpdateTokenAsync(Token token)
        {
            var tokenPrice = (await GetTokenAsync(token.Id)).PriceUSD;

            token.PriceUSD = (decimal)NormalizedPrice((double)token.PriceUSD, 2);

            await database.UpdateAsync(token);

            if (tokenPrice != token.PriceUSD)
            {
                OnTokenUpdated(token);
            }
        }

        public async Task<IEnumerable<Token>> GetTokensAsync(SortOrder sortOrder)
        {
            var tokens = new List<Token> { };

            switch (sortOrder)
            {
                case SortOrder.Alphabet:
                    tokens = await database.Table<Token>().OrderBy((t) => t.Symbol).ToListAsync();
                    break;
                case SortOrder.Rank:
                    tokens = await database.Table<Token>().OrderBy((t) => t.Rank).ToListAsync();
                    break;
            }

            return tokens;
        }

        public async Task<Token> GetTokenAsync(string id)
        {
            return await database.Table<Token>().Where((t) => t.Id == id).FirstOrDefaultAsync();            
        }

        public async Task UpdateRatesAsync(IEnumerable<Rate> rates)
        {
            await database.DeleteAllAsync<Rate>();
            await database.InsertAllAsync(rates);            
        }

        public async Task<Rate> GetRateAsync(string id)
        {
            var rate = await database.GetAsync<Rate>(id);
            return rate;
        }

        public async Task ResetAsync()
        {
            await database.DeleteAllAsync<Rate>();
            await database.DeleteAllAsync<Token>();

            await database.InsertAsync(Rate.Default());            
        }

        public void Configure()
        {
            database.DeleteAllAsync<Token>().Wait();
            var tokens = new List<Token>
            {
                new Token { Id = "bitcoin", Symbol = "BTC", Change24 = 0, PriceUSD = 0, Rank = 1 },
                new Token { Id = "ethereum", Symbol = "ETH", Change24 = 0, PriceUSD = 0, Rank = 2 },
                new Token { Id = "ripple", Symbol = "XRP", Change24 = 0, PriceUSD = 0, Rank = 3 },
            };
            database.InsertAllAsync(tokens).Wait();

            database.DeleteAllAsync<Rate>().Wait();
            database.InsertAsync(Rate.Default()).Wait();
        }

        #region Private

        private static double NormalizedPrice(double value, int numSignificantDigits)
        {
            if (value >= 1.0)
            {
                return Math.Round(value, numSignificantDigits);
            }
            else
            {                
                return value.RoundToSignificantDigits(numSignificantDigits);                
            }
        }

        private void OnTokenAdded(Token token)
        {
            TokenAdded?.Invoke(this, token);
        }

        private void OnTokenRemoved(Token token)
        {
            TokenRemoved?.Invoke(this, token);
        }

        private void OnTokenUpdated(Token token)
        {
            TokenUpdated?.Invoke(this, token);
        }

        #endregion
    }
}
