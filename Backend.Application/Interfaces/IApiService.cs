namespace Backend
{
    public interface IApiService
    {
        public Task<T?> GetAsync<T>(string requestUri, CancellationToken ct = default);

        public Task<TResponse?> PostAsync<TRequest, TResponse>(string requestUri, TRequest body, CancellationToken ct = default);
    }
}