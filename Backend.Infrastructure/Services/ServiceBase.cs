using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Net.Http.Json;
using System.Text.Json;

namespace Backend
{
    public abstract class ServiceBase //just for demo purposes
    {
        private readonly HttpClient _http;
        private readonly ILogger<ServiceBase> _logger;

        public ServiceBase(HttpClient http, ILogger<ServiceBase> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<T?> GetAsync<T>(string requestUri, CancellationToken ct = default)
        {
            Stopwatch sw = Stopwatch.StartNew();
            _logger.LogInformation("Sending GET request to {RequestUri}", requestUri);
            using var response = await _http.GetAsync(requestUri, ct).ConfigureAwait(false);
            sw.Stop();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("GET request to {RequestUri} failed with status code {StatusCode}, Elapsed MS: {Elapsed MS}", requestUri, response.StatusCode, sw.ElapsedMilliseconds);
                return default;
            }

            _logger.LogInformation("GET request to {RequestUri} succeeded, Elapsed MS: {Elapsed MS}", requestUri, sw.ElapsedMilliseconds);
            var result = await response.Content.ReadFromJsonAsync<T>(ct).ConfigureAwait(false);
            return result;
        }

        public async Task<TResponse?> PostAsync<TRequest, TResponse>(string requestUri, TRequest body, CancellationToken ct = default)
        {
            Stopwatch sw = Stopwatch.StartNew();
            _logger.LogInformation("Sending POST request to {RequestUri} with body: {RequestBody}", requestUri, JsonSerializer.Serialize(body));

            using var response = await _http.PostAsJsonAsync(requestUri, body, ct).ConfigureAwait(false);
            sw.Stop();

            if (!response.IsSuccessStatusCode)
            {
                _logger.LogError("POST request to {RequestUri} failed with status code {StatusCode}, Elapsed MS: {Elapsed MS}", requestUri, response.StatusCode, sw.ElapsedMilliseconds);
                return default;
            }

            _logger.LogInformation("POST request to {RequestUri} succeeded, Elapsed MS: {Elapsed MS}", requestUri, sw.ElapsedMilliseconds);

            var result = await response.Content.ReadFromJsonAsync<TResponse>(ct).ConfigureAwait(false);
            return result;
        }
    }
}
