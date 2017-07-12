using Microsoft.EntityFrameworkCore;
using TZDriver.Models.DataModels;

namespace TZDriver.DataStores
{
    public class DataContext : DbContext
    {
        public DbSet<TripData> TripDatas { get; set; }
        public DbSet<TripPoint> TripPoints { get; set; }
        public DbSet<FuelData> FuelDatas { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=tztaxidrive.db");
        }
    }
}
