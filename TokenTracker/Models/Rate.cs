using Newtonsoft.Json;
using SQLite;

namespace TokenTracker.Models
{
    /*
    {
      "id": "barbadian-dollar",
      "symbol": "BBD",
      "currencySymbol": "$",
      "type": "fiat",
      "rateUsd": "0.5000000000000000"
    }
     */

    public class Rate
    {
        public const string DEFAULT_RATE_ID = "united-states-dollar";

        [PrimaryKey]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("currencySymbol")]
        public string CurrencySymbol { get; set; }

        [JsonProperty("rateUsd")]
        public decimal RateUSD { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public static Rate Default()
        {
            return new Rate
            {
                Id = DEFAULT_RATE_ID,
                Symbol = "USD",
                CurrencySymbol = "$",
                RateUSD = new decimal(1.0),
                Type = "fiat",
            };
        }
    }
}
