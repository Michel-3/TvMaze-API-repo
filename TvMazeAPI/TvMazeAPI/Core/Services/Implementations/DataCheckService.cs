using Microsoft.EntityFrameworkCore;
using TvMazeAPI.Core.Models;
using TvMazeAPI.Core.Services.Interfaces;
using TvMazeAPI.Repository;

namespace TvMazeAPI.Core.Services.Implementations
{
    public class DataCheckService : IDataCheckService
    {
        private readonly TvMazeDbContext _dbContext;

        public DataCheckService(TvMazeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> MonthYearDataExistsAysnc(int year, int month)
        {
            return await _dbContext.Shows.AnyAsync(s => s.Year == year && s.Month == month);
        }

        public async Task<bool> ShowExistsAsync(int showId)
        {
            return await _dbContext.Shows.AnyAsync(s => s.ShowId == showId);
        }

        public async Task SaveShowInfoListAsync(List<ShowInfo> showInfoList)
        {
            foreach (var showInfo in showInfoList)
            {
                var show = new Show
                {
                    ShowId = showInfo.ShowId,
                    Day = int.Parse(showInfo.ShowAirDate.Split('-')[2]),
                    Month = int.Parse(showInfo.ShowAirDate.Split('-')[1]),
                    Year = int.Parse(showInfo.ShowAirDate.Split('-')[0]),
                    Actors = showInfo.Cast.Select(castMember => new Actor
                    {
                        ActorId = castMember.PersonId,
                        ActorName = castMember.PersonName,
                    }).ToList()
                };

                _dbContext.Shows.Add(show);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}
