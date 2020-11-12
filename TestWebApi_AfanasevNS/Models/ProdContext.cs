using Microsoft.EntityFrameworkCore;

namespace TestWebApi_AfanasevNS.Models
{
    public class ProdContext : DbContext
    {
        public DbSet<ProductUom> ProductUoms { get; set; }
        public DbSet<Product> Prod { get; set; }
        public DbSet<ProductMovements> Movements { get; set; }

        public ProdContext(DbContextOptions<ProdContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
