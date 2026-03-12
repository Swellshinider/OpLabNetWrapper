using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

public class InterestRate
{
    [JsonProperty("uid")]
    public string? Uid { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("value")]
    public decimal? Value { get; set; }

    [JsonProperty("updated_at")]
    public string? UpdatedAt { get; set; }
}
