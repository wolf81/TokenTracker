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
        private WebSocket webSocket;

        public event EventHandler<Dictionary<string, decimal>> TokensUpdated;

        public event EventHandler<ConnectionState> ConnectionStateChanged;

        public ConnectionState State { get; private set; } = ConnectionState.Disconnected;

        #region ITokenInfoService

        public void StartTokenUpdates()
        {            
            if (State == ConnectionState.Busy || State == ConnectionState.Connected) { return; }
            OnConnectionStateChanged(ConnectionState.Busy);

            webSocket.ConnectAsync();
        }

        public void StopTokenUpdates()
        {
            if (State == ConnectionState.Busy || State == ConnectionState.Disconnected) { return; }
            OnConnectionStateChanged(ConnectionState.Busy);

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

        public void Configure(IEnumerable<string> tokenIds)
        {
            // TODO: Throw exception if token ids length is 0?

            if (webSocket != null)
            {
                webSocket.Close();
                webSocket.OnMessage -= Handle_WebSocket_OnMessage;
                webSocket.OnError -= Handle_WebSocket_OnError;
                webSocket.OnOpen -= Handle_WebSocket_OnOpen;
                webSocket.OnClose -= Handle_WebSocket_OnClose;
                webSocket = null;
            }

            var query = string.Join(',', tokenIds);
            if (query.Length > 0)
            {
                webSocket = new WebSocket($"wss://ws.coincap.io/prices?assets={query}");
                webSocket.OnMessage += Handle_WebSocket_OnMessage;
                webSocket.OnOpen += Handle_WebSocket_OnOpen;
                webSocket.OnClose += Handle_WebSocket_OnClose;
                webSocket.OnError += Handle_WebSocket_OnError;
            }
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
            OnTokensUpdated(tokenPriceInfo);
        }

        private void Handle_WebSocket_OnOpen(object sender, EventArgs e)
        {
            Console.WriteLine("[WS] open");

            OnConnectionStateChanged(ConnectionState.Connected);
        }

        private void Handle_WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            Console.WriteLine("[WS] close");

            OnConnectionStateChanged(ConnectionState.Disconnected);
        }

        private void Handle_WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            Console.WriteLine($"[WS] error {e?.Message}");
        }

        private void OnConnectionStateChanged(ConnectionState state)
        {
            State = state;

            ConnectionStateChanged?.Invoke(this, state);
        }

        private void OnTokensUpdated(Dictionary<string, decimal> tokenPriceInfo)
        {
            TokensUpdated?.Invoke(this, tokenPriceInfo);
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
