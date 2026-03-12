using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

/// <summary>
/// Basic instrument details.
/// </summary>
public class Instrument
{
    [JsonProperty("symbol")]
    public string? Symbol { get; set; }

    [JsonProperty("type")]
    public string? Type { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("open")]
    public decimal? Open { get; set; }

    [JsonProperty("high")]
    public decimal? High { get; set; }

    [JsonProperty("low")]
    public decimal? Low { get; set; }

    [JsonProperty("close")]
    public decimal? Close { get; set; }
}
