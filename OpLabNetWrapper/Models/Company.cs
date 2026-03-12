using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

public class Company
{
    [JsonProperty("symbol")]
    public string? Symbol { get; set; }

    [JsonProperty("cnpj")]
    public string? Cnpj { get; set; }
}
