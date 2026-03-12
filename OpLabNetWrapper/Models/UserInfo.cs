using Newtonsoft.Json;

namespace OpLabNetWrapper.Models;

/// <summary>
/// Partial user info returned by OpLab authentication endpoints.
/// </summary>
public class UserInfo
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string? Name { get; set; }

    [JsonProperty("email")]
    public string? Email { get; set; }

    [JsonProperty("access-token")]
    public string? AccessToken { get; set; }

    [JsonProperty("data-access")]
    public string? DataAccess { get; set; }

    [JsonProperty("display-name")]
    public string? DisplayName { get; set; }
}
