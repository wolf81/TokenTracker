using System;
using Newtonsoft.Json;
using SQLite;

namespace TokenTracker.Models
{
    /*
    {
      "id": "ethereum",
      "rank": "2",
      "symbol": "ETH",
      "name": "Ethereum",
      "supply": "112089367.4990000000000000",
      "maxSupply": null,
      "marketCapUsd": "44320549405.6846347668155796",
      "volumeUsd24Hr": "2388474225.0445657723499220",
      "priceUsd": "395.4036889902161189",
      "changePercent24Hr": "-0.7028004511521917",
      "vwap24Hr": "404.0870367372976516"
    },
     */

    public class Token : ICloneable
    {
        [PrimaryKey]
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("rank")]
        public int Rank { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("priceUsd", NullValueHandling = NullValueHandling.Ignore)]
        public decimal PriceUSD { get; set; } = 0;

        [JsonProperty("changePercent24Hr", NullValueHandling = NullValueHandling.Ignore)]
        public decimal Change24 { get; set; } = 0;

        public object Clone()
        {
            return new Token
            {
                Id = Id,
                Symbol = Symbol,
                Name = Name,
                PriceUSD = PriceUSD,
                Change24 = Change24,
            };
        }

        public static Token Dummy = new Token { Id = Guid.NewGuid().ToString(), Rank = int.MaxValue };
    }
}
