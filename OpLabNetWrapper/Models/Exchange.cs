using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

public class Exchange
{
    [JsonProperty("uid")]
    public string? Uid { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("opensAt")]
    public string? OpensAt { get; set; }

    [JsonProperty("closesAt")]
    public string? ClosesAt { get; set; }
}
