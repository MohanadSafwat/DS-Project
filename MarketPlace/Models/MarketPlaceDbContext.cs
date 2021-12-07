using Microsoft.EntityFrameworkCore;

namespace MarketPlace.Models
{
    public class MarketPlaceDbContext :DbContext
    {
        public MarketPlaceDbContext(DbContextOptions<MarketPlaceDbContext> options):base(options)
        {
            
        }

        public DbSet<Product> Products { get; set; }

    }
}
