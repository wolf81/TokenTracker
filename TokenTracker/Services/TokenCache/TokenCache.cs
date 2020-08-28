using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using SQLite;
using TokenTracker.Models;

namespace TokenTracker.Services.TokenCache
{
    public class TokenCache : ITokenCache
    {
        public event EventHandler<Token> TokenAdded;

        public event EventHandler<Token> TokenRemoved;

        public event EventHandler<Token> TokenUpdated;

        private readonly SQLiteAsyncConnection database;

        private static readonly string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cache.db");

        public TokenCache()
        {
            Console.WriteLine($"Cache path: {filepath}");

            database = new SQLiteAsyncConnection(filepath);
            database.CreateTableAsync<Token>().Wait();
        }

        public async Task AddTokenAsync(Token token)
        {
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
            await database.UpdateAsync(token);

            OnTokenUpdated(token);
        }

        public async Task<IEnumerable<Token>> GetTokensAsync()
        {
            return await database.Table<Token>().ToListAsync();
        }

        public async Task<Token> GetTokenAsync(string id)
        {
            return await database.Table<Token>().Where((t) => t.Id == id).FirstOrDefaultAsync();            
        }

        public void Configure()
        {
            database.DeleteAllAsync<Token>().Wait();

            var tokens = new List<Token>
            {
                new Token { Id = "bitcoin", Symbol = "BTC", Change24 = 0, PriceUSD = 0 },
                new Token { Id = "ethereum", Symbol = "ETH", Change24 = 0, PriceUSD = 0 },
                new Token { Id = "ripple", Symbol = "XRP", Change24 = 0, PriceUSD = 0 },
            };
            database.InsertAllAsync(tokens).Wait();
        }

        #region Private

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
