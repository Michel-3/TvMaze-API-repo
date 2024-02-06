using TvMazeAPI.Core.Models;

namespace TvMazeAPI.Core.Services.Interfaces
{
    public interface IApiDataService
    {
        Task<List<ShowInfo>> RetrieveShowsAsync(int year, int month);
    }
}