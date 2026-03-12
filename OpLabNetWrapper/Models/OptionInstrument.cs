using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

/// <summary>
/// Option instrument brief data.
/// </summary>
public class OptionInstrument
{
    [JsonProperty("symbol")]
    public string? Symbol { get; set; }

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

    [JsonProperty("bid")]
    public decimal? Bid { get; set; }

    [JsonProperty("ask")]
    public decimal? Ask { get; set; }

    [JsonProperty("category")]
    public string? Category { get; set; }

    [JsonProperty("due_date")]
    public string? DueDate { get; set; }
}
