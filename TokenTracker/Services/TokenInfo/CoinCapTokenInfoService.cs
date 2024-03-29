﻿using System;
using System.Threading.Tasks;
using System.Net.Http;
using WebSocketSharp;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;
using TokenTracker.Models;
using System.Linq;

namespace TokenTracker.Services
{
    public class CoinCapTokenInfoService : ITokenInfoService
    {
        private static void Log(string message) { Console.WriteLine($"[WS] {message}"); }

        private const string API_ENDPOINT = "https://api.coincap.io/v2";
        private const string WS_ENDPOINT = "wss://ws.coincap.io";

        private WebSocket webSocket;

        public event EventHandler<Dictionary<string, decimal>> TokensUpdated;

        public event EventHandler<ConnectionState> ConnectionStateChanged;

        public bool IsConfigured => webSocket != null;

        #region ITokenInfoService

        public ConnectionState State { get; private set; } = ConnectionState.Disconnected;

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
            using (var response = await httpClient.GetAsync($"{API_ENDPOINT}/assets"))
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
            using (var response = await httpClient.GetAsync($"{API_ENDPOINT}/assets?search={tokenIdOrSymbol}"))
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
            using (var response = await httpClient.GetAsync($"{API_ENDPOINT}/assets/{tokenId}/history?interval={intervalParam}&start={start}&end={end}"))
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
                webSocket = new WebSocket($"{WS_ENDPOINT}/prices?assets={query}")
                {
                    WaitTime = TimeSpan.FromMilliseconds(3_000),                    
                };
                webSocket.OnMessage += Handle_WebSocket_OnMessage;
                webSocket.OnOpen += Handle_WebSocket_OnOpen;
                webSocket.OnClose += Handle_WebSocket_OnClose;
                webSocket.OnError += Handle_WebSocket_OnError;
            }
        }

        public async Task<IEnumerable<Rate>> GetRatesAsync()
        {
            IEnumerable<Rate> result = null;

            using (var httpClient = CreateHttpClient())
            using (var response = await httpClient.GetAsync($"{API_ENDPOINT}/rates"))
            {
                result = await HandleResponseAsync<IEnumerable<Rate>>(response);
                Console.WriteLine($"{response.StatusCode} {result}");
            }

            var rates = result.Where((t) => t.Type == "fiat" || t.Symbol == "BTC").OrderBy((t) => t.Symbol);
            
            return rates;
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

        private static HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        private void Handle_WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            Log($"message: {e.Data}");

            var tokenPriceInfo = JsonConvert.DeserializeObject<Dictionary<string, decimal>>(e.Data);            
            OnTokensUpdated(tokenPriceInfo);
        }

        private async void Handle_WebSocket_OnOpen(object sender, EventArgs e)
        {
            Log("open");

            OnConnectionStateChanged(ConnectionState.Connected);

            await MonitorConnectionAsync();
        }

        private async void Handle_WebSocket_OnClose(object sender, CloseEventArgs e)
        {
            Log("close");

            OnConnectionStateChanged(ConnectionState.Disconnected);

            await TryReconnectAsync();
        }

        private void Handle_WebSocket_OnError(object sender, ErrorEventArgs e)
        {
            Log($"error: {e?.Message}");
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

        private async Task MonitorConnectionAsync()
        {
            while (true)
            {
                if (!webSocket.IsAlive) {
                    if (webSocket.ReadyState != WebSocketState.Closed)
                    {
                        Log("closing ...");
                        webSocket.Close();
                    }

                    break;
                }

                await Task.Delay(3_000);
            }
        }

        private async Task TryReconnectAsync()
        {
            await Task.Delay(5_000);

            while (true)
            {
                if (webSocket != null && webSocket.IsAlive) {
                    Log("connected");
                    break;
                }

                StartTokenUpdates();

                await Task.Delay(3_000);
            }
        }

        #endregion
    }
}
