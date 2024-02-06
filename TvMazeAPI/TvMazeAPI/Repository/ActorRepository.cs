using Microsoft.EntityFrameworkCore;


namespace TvMazeAPI.Repository
{
    public class ActorRepository
    {
        private readonly TvMazeDbContext _dbContext;

        public ActorRepository(TvMazeDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<int> GetTotalActors(int month, int year)
        {
            var totalActors = await _dbContext.Actors
                .Join(_dbContext.Shows,
                    actor => actor.ShowID,
                    show => show.ID,
                    (actor, show) => new { actor, show })
                .Where(joined => joined.show.Year == year && joined.show.Month == month)
                .CountAsync();

            return totalActors;
        }

        public async Task<IEnumerable<object>> Calculatepercentage(int month, int year, int totalActors)
        {
            var topActors = await _dbContext.Actors
                .Join(_dbContext.Shows,
                actor => actor.ShowID,
                show => show.ID,
                   (actor, show) => new { actor, show })
                .Where(joined => joined.show.Year == year && joined.show.Month == month)
                .GroupBy(joined => joined.actor.ActorId)
                .Select(group => new
                {
                    ActorId = group.Key,
                    Percentage = (double)group.Count() / totalActors * 100,
                    ActorName = group.First().actor.ActorName ?? "Unknown"
                })
                .OrderByDescending(x => x.Percentage)
                .Take(10)
                .ToListAsync();

            return topActors.ToList();
        }
    }
}
