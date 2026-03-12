using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace OpLabNetWrapper;

/// <summary>
/// Minimal OpLab API client. Handles authentication and common market/domain calls.
/// </summary>
public sealed class OpLabClient : IDisposable
{
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    /// <summary>
    /// Creates a new <see cref="OpLabClient"/>.
    /// </summary>
    /// <param name="baseUrl">API base URL. Default: https://api.oplab.com.br/v3</param>
    public OpLabClient(string baseUrl = "https://api.oplab.com.br/v3")
    {
        _baseUrl = baseUrl;
        _http = new HttpClient();
        _http.DefaultRequestHeaders.Accept.Clear();
        _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    /// <summary>
    /// Sets the access token to be sent as `Access-Token` header on requests.
    /// </summary>
    /// <param name="token">Access token value.</param>
    public void SetAccessToken(string? token)
    {
        if (string.IsNullOrEmpty(token))
        {
            if (_http.DefaultRequestHeaders.Contains("Access-Token"))
                _http.DefaultRequestHeaders.Remove("Access-Token");

            return;
        }

        if (_http.DefaultRequestHeaders.Contains("Access-Token"))
            _http.DefaultRequestHeaders.Remove("Access-Token");
        _http.DefaultRequestHeaders.Add("Access-Token", token);
    }

    private async Task<T?> GetAsync<T>(string path)
    {
        using var resp = await _http.GetAsync($"{_baseUrl}/{path}").ConfigureAwait(false);
        resp.EnsureSuccessStatusCode();

        var str = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonConvert.DeserializeObject<T?>(str);
    }

    private async Task<T?> PostAsync<T>(string path, object? payload)
    {
        var body = payload is null ? null : new StringContent(JsonConvert.SerializeObject(payload), System.Text.Encoding.UTF8, "application/json");
        using var resp = await _http.PostAsync(path, body).ConfigureAwait(false);
        resp.EnsureSuccessStatusCode();
        var str = await resp.Content.ReadAsStringAsync().ConfigureAwait(false);
        return JsonConvert.DeserializeObject<T?>(str);
    }

    private static string BuildQueryString(IDictionary<string, string?>? qs)
    {
        if (qs == null || qs.Count == 0)
            return string.Empty;

        var parts = new List<string>();

        foreach (var kv in qs)
        {
            if (string.IsNullOrEmpty(kv.Value)) continue;
            parts.Add($"{Uri.EscapeDataString(kv.Key)}={Uri.EscapeDataString(kv.Value)}");
        }

        return parts.Count == 0 ? string.Empty : "?" + string.Join("&", parts);
    }

    /// <summary>
    /// Authenticates a user and returns the <see cref="Models.UserInfo"/>. On success the client's access token is set.
    /// </summary>
    /// <param name="email">User email.</param>
    /// <param name="password">User password.</param>
    /// <param name="context">Optional context query param (default: "default").</param>
    public async Task<Models.UserInfo?> AuthenticateAsync(string email, string password, string? context = "default")
    {
        var query = string.IsNullOrEmpty(context) ? string.Empty : $"?context={Uri.EscapeDataString(context)}";
        var payload = new { email, password };
        var result = await PostAsync<Models.UserInfo>($"domain/users/authenticate{query}", payload).ConfigureAwait(false);
        if (result?.AccessToken != null)
            SetAccessToken(result.AccessToken);
        return result;
    }

    /// <summary>
    /// Gets quotes for a comma-separated list of tickers.
    /// </summary>
    /// <param name="tickers">Comma-separated tickers (e.g. "PETR4,ABEV3").</param>
    public Task<List<Models.Quote>?> GetQuotesAsync(string tickers)
    {
        var path = $"market/quote?tickers={Uri.EscapeDataString(tickers)}";
        return GetAsync<List<Models.Quote>>(path);
    }

    /// <summary>
    /// Gets instrument details for a symbol.
    /// </summary>
    public Task<Models.Instrument?> GetInstrumentAsync(string symbol)
    {
        var path = $"market/instruments/{Uri.EscapeDataString(symbol)}";
        return GetAsync<Models.Instrument>(path);
    }

    /// <summary>
    /// Gets market server status.
    /// </summary>
    public Task<Models.MarketStatus?> GetMarketExchangeList()
    {
        return GetAsync<Models.MarketStatus>("market/exchanges");
    }

    /// <summary>
    /// Lists interest rates.
    /// </summary>
    public Task<List<Models.InterestRate>?> GetInterestRatesAsync()
    {
        return GetAsync<List<Models.InterestRate>>("market/interest_rates");
    }

    /// <summary>
    /// Gets a single interest rate by id.
    /// </summary>
    public Task<Models.InterestRate?> GetInterestRateAsync(string id)
    {
        var path = $"market/interest_rates/{Uri.EscapeDataString(id)}";
        return GetAsync<Models.InterestRate>(path);
    }

    /// <summary>
    /// Lists exchanges.
    /// </summary>
    public Task<List<Models.Exchange>?> GetExchangesAsync()
    {
        return GetAsync<List<Models.Exchange>>("market/exchanges");
    }

    /// <summary>
    /// Gets a single exchange by uid.
    /// </summary>
    public Task<Models.Exchange?> GetExchangeAsync(string uid)
    {
        var path = $"market/exchanges/{Uri.EscapeDataString(uid)}";
        return GetAsync<Models.Exchange>(path);
    }

    /// <summary>
    /// Searches instruments with the given parameters (wrapper for `market/instruments/search`).
    /// </summary>
    public Task<List<Models.InstrumentSearch>?> SearchInstrumentsAsync(string expr, int? limit = null, string? type = null, bool? hasOptions = null, string? category = null, bool? addInfo = null)
    {
        var qs = new Dictionary<string, string?> { ["expr"] = expr };
        if (limit != null) qs["limit"] = limit.ToString();
        if (!string.IsNullOrEmpty(type)) qs["type"] = type;
        if (hasOptions != null) qs["has_options"] = hasOptions.Value ? "true" : "false";
        if (!string.IsNullOrEmpty(category)) qs["category"] = category;
        if (addInfo != null) qs["add_info"] = addInfo.Value ? "true" : "false";
        var path = "market/instruments/search" + BuildQueryString(qs);
        return GetAsync<List<Models.InstrumentSearch>>(path);
    }

    /// <summary>
    /// Lists series for an instrument.
    /// </summary>
    public Task<object?> GetInstrumentSeriesAsync(string symbol, bool? bs = null, double? irate = null)
    {
        var qs = new Dictionary<string, string?>();
        if (bs != null) qs["bs"] = bs.Value ? "true" : "false";
        if (irate != null) qs["irate"] = irate.Value.ToString(System.Globalization.CultureInfo.InvariantCulture);
        var path = $"market/instruments/series/{Uri.EscapeDataString(symbol)}" + BuildQueryString(qs);
        return GetAsync<object>(path);
    }

    /// <summary>
    /// Gets instruments details for a list of tickers (market/instruments?tickers=...)
    /// </summary>
    public Task<List<Models.Instrument>?> GetInstrumentsDetailAsync(string tickers)
    {
        var path = $"market/instruments?tickers={Uri.EscapeDataString(tickers)}";
        return GetAsync<List<Models.Instrument>>(path);
    }

    /// <summary>
    /// Black-Scholes calculator endpoint wrapper: `market/options/bs`.
    /// </summary>
    public Task<object?> GetOptionsBsAsync(IDictionary<string, string?> queryParams)
    {
        var path = "market/options/bs" + BuildQueryString(queryParams as Dictionary<string, string?>);
        return GetAsync<object>(path);
    }

    /// <summary>
    /// Lists options powders (market/options/powders).
    /// </summary>
    public Task<List<object>?> GetOptionsPowdersAsync()
    {
        return GetAsync<List<object>>("market/options/powders");
    }

    /// <summary>
    /// Lists stocks (market/stocks) with optional query string parameters.
    /// </summary>
    public Task<List<Models.Stock>?> GetStocksAsync(IDictionary<string, string?>? queryParams = null)
    {
        var path = "market/stocks" + BuildQueryString(queryParams);
        return GetAsync<List<Models.Stock>>(path);
    }

    /// <summary>
    /// Lists all stocks (market/stocks/all) with optional query parameters.
    /// </summary>
    public Task<List<Models.Stock>?> GetStocksAllAsync(IDictionary<string, string?>? queryParams = null)
    {
        var path = "market/stocks/all" + BuildQueryString(queryParams);
        return GetAsync<List<Models.Stock>>(path);
    }

    /// <summary>
    /// Gets companies details (market/companies).
    /// </summary>
    public Task<List<Models.Company>?> GetCompaniesAsync(string symbols, string? includes = null)
    {
        var qs = new Dictionary<string, string?> { ["symbols"] = symbols };
        if (!string.IsNullOrEmpty(includes)) qs["includes"] = includes;
        var path = "market/companies" + BuildQueryString(qs);
        return GetAsync<List<Models.Company>>(path);
    }

    /// <summary>
    /// Rankings - highest options volume (realtime).
    /// </summary>
    public Task<List<object>?> GetHighestOptionsVolumeAsync(string? orderBy = null, int? limit = null)
    {
        var qs = new Dictionary<string, string?>();
        if (!string.IsNullOrEmpty(orderBy)) qs["order_by"] = orderBy;
        if (limit != null) qs["limit"] = limit.ToString();
        var path = "market/statistics/realtime/highest_options_volume" + BuildQueryString(qs);
        return GetAsync<List<object>>(path);
    }

    /// <summary>
    /// Rankings - OpLab score.
    /// </summary>
    public Task<List<object>?> GetOplabScoreRankingAsync(IDictionary<string, string?>? queryParams = null)
    {
        var path = "market/statistics/ranking/oplab_score" + BuildQueryString(queryParams);
        return GetAsync<List<object>>(path);
    }

    /// <summary>
    /// Gets options list for an underlying symbol.
    /// </summary>
    public Task<List<Models.OptionInstrument>?> GetOptionsAsync(string symbol)
    {
        var path = $"market/options/{Uri.EscapeDataString(symbol)}";
        return GetAsync<List<Models.OptionInstrument>>(path);
    }

    /// <summary>
    /// Gets historical data for a symbol and resolution.
    /// </summary>
    /// <param name="symbol">Instrument symbol.</param>
    /// <param name="resolution">Resolution string (e.g. "1d", "1h").</param>
    /// <param name="from">Start date/time ISO or timestamp.</param>
    /// <param name="to">End date/time ISO or timestamp.</param>
    public Task<object?> GetHistoricalAsync(string symbol, string resolution, string from, string to)
    {
        var path = $"market/historical/{Uri.EscapeDataString(symbol)}/{Uri.EscapeDataString(resolution)}?from={Uri.EscapeDataString(from)}&to={Uri.EscapeDataString(to)}";
        return GetAsync<object>(path);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _http?.Dispose();
    }
}
