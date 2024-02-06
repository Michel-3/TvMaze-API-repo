using TvMazeAPI.Core.Models;

namespace TvMazeAPI.Core.Services.Interfaces
{
    public interface IDataCheckService
    {
        Task<bool> MonthYearDataExistsAysnc(int year, int month);
        Task SaveShowInfoListAsync(List<ShowInfo> showInfoList);
        Task<bool> ShowExistsAsync(int showId);
    }
}