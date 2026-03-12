using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

public class InstrumentSearch
{
    [JsonProperty("symbol")]
    public string? Symbol { get; set; }

    [JsonProperty("full_name")]
    public string? FullName { get; set; }

    [JsonProperty("description")]
    public string? Description { get; set; }
}
