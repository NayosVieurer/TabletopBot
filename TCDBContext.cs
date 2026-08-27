using Microsoft.EntityFrameworkCore;
using TabletobClubBot.Models.Config;

namespace TabletobClubBot
{
    public class TCDBContext(DbContextOptions<TCDBContext> options) : DbContext(options)
    {
        public DbSet<HQConfig> hqConfigs { get; set; }
    }
}
