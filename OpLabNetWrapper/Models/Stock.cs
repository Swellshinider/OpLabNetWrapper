using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

public class Stock
{
    [JsonProperty("symbol")]
    public string? Symbol { get; set; }

    [JsonProperty("long_name")]
    public string? LongName { get; set; }

    [JsonProperty("open")]
    public decimal? Open { get; set; }

    [JsonProperty("close")]
    public decimal? Close { get; set; }

    [JsonProperty("variation")]
    public decimal? Variation { get; set; }
}
