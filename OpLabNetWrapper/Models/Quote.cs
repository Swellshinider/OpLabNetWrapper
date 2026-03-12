using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

/// <summary>
/// Market quote for an instrument.
/// </summary>
public class Quote
{
    [JsonProperty("symbol")]
    public string? Symbol { get; set; }

    [JsonProperty("close")]
    public decimal? Close { get; set; }

    [JsonProperty("variation")]
    public decimal? Variation { get; set; }

    [JsonProperty("bid")]
    public decimal? Bid { get; set; }

    [JsonProperty("ask")]
    public decimal? Ask { get; set; }

    [JsonProperty("time")]
    public long? Time { get; set; }
}
