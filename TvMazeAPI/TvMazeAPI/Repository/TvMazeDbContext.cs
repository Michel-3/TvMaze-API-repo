using Microsoft.EntityFrameworkCore;

namespace TvMazeAPI.Repository
{
    public class TvMazeDbContext: DbContext
    {
        public TvMazeDbContext(DbContextOptions<TvMazeDbContext> options) : base(options) { }
        public DbSet<Show> Shows { get; set; }
        //public DbSet<Actor> Actors { get; set; }
    }

    public class Show
    {
        public int ShowId { get; set; }
        public string ShowName { get; set; }

        //public List<Post> Posts { get; } = new();
    }
}
