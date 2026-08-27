using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TabletobClubBot
{
    public class TCDBContextFactory : IDesignTimeDbContextFactory<TCDBContext>
    {
        public TCDBContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<TCDBContext>();

            optionsBuilder.UseNpgsql("Host=localhost;Database=TabletopClub;Username=postgres;Password=sfight");

            return new TCDBContext(optionsBuilder.Options);
        }
    }
}
