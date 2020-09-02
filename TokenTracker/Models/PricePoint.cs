using Newtonsoft.Json;

namespace TokenTracker.Models
{
    public class PricePoint
    {
        [JsonProperty("priceUsd")]
        public decimal PriceUSD { get; set; }

        [JsonProperty("time")]
        public long Time { get; set; }
    }
}
