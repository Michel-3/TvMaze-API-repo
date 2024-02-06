namespace TvMazeAPI.Core.Services.Interfaces
{
    public interface IRateLimiter
    {
        Task<string> ApiRateLimiter(string apiUrl);
    }
}