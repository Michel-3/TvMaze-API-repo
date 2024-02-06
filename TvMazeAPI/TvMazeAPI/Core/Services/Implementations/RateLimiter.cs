using TvMazeAPI.Core.Services.Interfaces;

namespace TvMazeAPI.Core.Services.Implementations
{
    public class RateLimiter : IRateLimiter
    {
        private readonly HttpClient _httpClient;
        private const int rateLimitDelay = 500;
        public RateLimiter(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://api.tvmaze.com/");
        }
        public async Task<string> ApiRateLimiter(string apiUrl)
        {
            await Task.Delay(rateLimitDelay);

            var response = await _httpClient.GetStringAsync(apiUrl);

            return response;
        }
    }
}
