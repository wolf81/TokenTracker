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

            webSocket?.ConnectAsync();
        }

        public void StopTokenUpdates()
        {
            if (State == ConnectionState.Busy || State == ConnectionState.Disconnected) { return; }
            OnConnectionStateChanged(ConnectionState.Busy);

            webSocket?.CloseAsync();
        }

        public async Task<IEnumerable<Token>> GetTokensAsync()
        {
            IEnumerable<Token> result = null;

            using (var httpClient = CreateHttpClient())
            using (var response = await httpClient.GetAsync("https://api.coincap.io/v2/assets"))
            {
                result = await HandleResponseAsync<IEnumerable<Token>>(response);
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
                result = await HandleResponseAsync<IEnumerable<Token>>(response);
                Console.WriteLine($"{response.StatusCode} {result}");
            }

            return result;
        }

        public async Task<IEnumerable<PricePoint>> GetTokenHistoryAsync(string tokenId, Interval interval)
        {
            IEnumerable<PricePoint> result = null;

            var startTime = DateTime.Now;

            switch (interval)
            {
                case Interval.Day: startTime = DateTime.Now.AddHours(-25); break; 
                case Interval.Week: startTime= DateTime.Now.AddDays(-8); break; // retrieve 7 days
                case Interval.Month: startTime = DateTime.Now.AddDays(-32); break; // retrieve 31 days
                case Interval.Year: startTime = DateTime.Now.AddDays(-361); break; // retrieve 360 days
            }

            var start = new DateTimeOffset(startTime).ToUnixTimeMilliseconds();
            var end = new DateTimeOffset(DateTime.Now).ToUnixTimeMilliseconds();
            var intervalParam = interval == Interval.Day ? "h1" : "d1";

            using (var httpClient = CreateHttpClient())
            using (var response = await httpClient.GetAsync($"https://api.coincap.io/v2/assets/{tokenId}/history?interval={intervalParam}&start={start}&end={end}"))
            {
                result = await HandleResponseAsync<IEnumerable<PricePoint>>(response);
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

        private async Task<T> HandleResponseAsync<T>(HttpResponseMessage response) 
        {
            var result = default(T);
            var serialized = await response.Content.ReadAsStringAsync();

            switch (response.StatusCode)
            {
                case System.Net.HttpStatusCode.OK:
                    var dataResult = await Task.Run(() => JsonConvert.DeserializeObject<CoinCapDataResponse<T>>(serialized));
                    result = dataResult.Data;
                    break;
                default:
                    var errorResult = await Task.Run(() => JsonConvert.DeserializeObject<CoinCapErrorResponse>(serialized));
                    throw new CoinCapServiceException(errorResult.Error, response.StatusCode);
            }

            return result;
        }

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

        public class CoinCapServiceException : Exception
        {
            public System.Net.HttpStatusCode HttpStatusCode { get; private set; }

            public CoinCapServiceException(string message, System.Net.HttpStatusCode httpStatusCode) : base(message)
            {
                HttpStatusCode = httpStatusCode;
            }
        }

        private class CoinCapErrorResponse
        {
            [JsonProperty("error")]
            public string Error { get; set; }

            [JsonProperty("timestamp")]
            public long Timestamp { get; set; }
        }

        private class CoinCapDataResponse<T>
        {
            [JsonProperty("data")]
            public T Data { get; set; }

            [JsonProperty("timestamp")]
            public long Timestamp { get; set; }
        }

        #endregion
    }
}
