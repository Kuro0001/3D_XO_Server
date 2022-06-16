using Microsoft.EntityFrameworkCore;

namespace _3D_XO_Server.Models
{
    public class DBContext : DbContext
    {
        public DbSet<Game> Games { get; set; }
        public DbSet<NewsBlock> NewsBlocks { get; set; }
        public DbSet<SearchingUser> SearchingUsers { get; set; }
        public DbSet<GamesState> GamesStates { get; set; }
        public DbSet<GameResult> GameResults { get; set; }
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        { }
    }
}
