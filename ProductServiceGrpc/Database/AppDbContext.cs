using EfCoreTutorial.Entity.ECommerceModels;
using Microsoft.EntityFrameworkCore;

namespace ProductServiceGrpc.Database
{
    public class AppDbContext :DbContext
    {
        public DbSet<ProductCategory> ProductCategories {get; set;}
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<ProductModel> Products { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductCategory>()
                .HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<ProductModel>().HasQueryFilter(x => !x.IsDeleted);

            modelBuilder.Entity<Seller>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
