namespace TvMazeAPI.Core.Services
{
    public class RateLimiter
    {
        private readonly HttpClient _httpClient;
        public RateLimiter(HttpClient httpClient) 
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("http://api.tvmaze.com/");
        }
        public async Task Delay(int millisecondsDelay)
        {
            await Task.Delay(millisecondsDelay);
        }
        public async Task<string> ApiRateLimiter(string apiUrl)
        {
            await Task.Delay(500);

            var response = await _httpClient.GetStringAsync(apiUrl);

            return response;
        }
    }
}
