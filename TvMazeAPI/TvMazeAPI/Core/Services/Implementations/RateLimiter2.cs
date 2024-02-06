using TvMazeAPI.Core.Services.Interfaces;

namespace TvMazeAPI.Core.Services.Implementations
{
    public class RateLimiter2 : IRateLimiter
    {
        private readonly HttpClient _httpClient;
        private const int rateLimitDelay = 1000;
        public RateLimiter2(HttpClient httpClient)
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
