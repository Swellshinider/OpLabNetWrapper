using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

/// <summary>
/// Minimal order representation.
/// </summary>
public class Order
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("symbol")]
    public string? Symbol { get; set; }

    [JsonProperty("price")]
    public decimal? Price { get; set; }

    [JsonProperty("amount")]
    public decimal? Amount { get; set; }

    [JsonProperty("status")]
    public string? Status { get; set; }
}
