using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

/// <summary>
/// Minimal portfolio representation.
/// </summary>
public class Portfolio
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("user_id")]
    public int UserId { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }
}
