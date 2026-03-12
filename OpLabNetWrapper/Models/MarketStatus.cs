using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

public class MarketStatus
{
    [JsonProperty("server_time")]
    public string? ServerTime { get; set; }

    [JsonProperty("market_status")]
    public string? MarketState { get; set; }
}
