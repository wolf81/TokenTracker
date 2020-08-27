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
        private readonly SQLiteAsyncConnection database;

        private static readonly string filepath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "cache.db");

        public TokenCache()
        {            
            database = new SQLiteAsyncConnection(filepath);
            database.CreateTableAsync<Token>().Wait();
        }

        public async Task AddTokenAsync(Token token)
        {
            await database.InsertOrReplaceAsync(token);
        }

        public async Task RemoveTokenAsync(Token token)
        {
            await database.DeleteAsync(token);
        }

        public async Task<IEnumerable<Token>> GetTokensAsync()
        {
            return await database.Table<Token>().ToListAsync();
        }
    }
}
