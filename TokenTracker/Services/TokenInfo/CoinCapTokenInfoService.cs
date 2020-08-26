using System;
using System.Threading.Tasks;
using System.Net.Http;
using WebSocketSharp;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using TokenTracker.Models;

namespace TokenTracker.Services
{
    public class CoinCapTokenInfoService : ITokenInfoService
    {
        private readonly WebSocket webSocket;

        public event EventHandler<Dictionary<string, decimal>> TokensUpdated;

        public CoinCapTokenInfoService()
        {
            webSocket = new WebSocket("wss://ws.coincap.io/prices?assets=bitcoin,ethereum,monero,litecoin");
            webSocket.OnMessage += Handle_WebSocket_OnMessage;
            webSocket.OnOpen += Handle_WebSocket_OnOpen;
            webSocket.OnClose += Handle_WebSocket_OnClose;
            webSocket.OnError += Handle_WebSocket_OnError;
        }

        #region ITokenInfoService

        public void StartTokenUpdates()
        {
            webSocket.ConnectAsync();
        }

        public void StopTokenUpdates()
        {
            webSocket.CloseAsync();
        }

        public async Task<IEnumerable<Token>> GetTokensAsync()
        {
            IEnumerable<Token> result = null;

            using (var httpClient = CreateHttpClient())
            using (var response = await httpClient.GetAsync("https://api.coincap.io/v2/assets"))
            {
                var serialized = await response.Content.ReadAsStringAsync();
                var wrappedResult = await Task.Run(() => JsonConvert.DeserializeObject<CoinCapResponse<IEnumerable<Token>>>(serialized));
                result = wrappedResult.Data;

                Console.WriteLine($"{response.StatusCode} {result}");
            }

            return result;            
        }

        public async Task<IEnumerable<Token>> GetTokensAsync(string tokenIdOrSymbol)
        {
            IEnumerable<Token> result = null;

            using (var httpClient = CreateHttpClient())
            using (var response = await httpClient.GetAsync($"https://api.coincap.io/v2/assets?search={tokenIdOrSymbol}"))
            {
                var serialized = await response.Content.ReadAsStringAsync();
                var wrappedResult = await Task.Run(() => JsonConvert.DeserializeObject<CoinCapResponse<IEnumerable<Token>>>(serialized));
                result = wrappedResult.Data;                

                Console.WriteLine($"{response.StatusCode} {result}");
            }

            return result;
        }

        #endregion

        #region Private

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        private void Handle_WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            Console.WriteLine($"[WS] message: {e.Data}");

            var tokenPriceInfo = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(e.Data);
            TokensUpdated?.Invoke(this, tokenPriceInfo);
        }

        private void Handle_WebSocket_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("[WS] open");
        }

        private void Handle_WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine("[WS] close");
        }

        private void Handle_WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"[WS] error {e?.Message}");
        }

        #endregion

        #region CoinCapResponse

        private class CoinCapResponse<T>
        {
            [JsonProperty("data")]
            public T Data { get; set; }

            [JsonProperty("timestamp")]
            public long Timestamp { get; set; }
        }

        #endregion
    }
}
