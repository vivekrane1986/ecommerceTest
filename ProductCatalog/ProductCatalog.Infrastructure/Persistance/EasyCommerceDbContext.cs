using Microsoft.EntityFrameworkCore;
using ProductCatalog.Infrastructure.DBEntities;

namespace ProductCatalog.Infrastructure.Persistance;

public class EasyCommerceDbContext:DbContext
{
    public EasyCommerceDbContext(DbContextOptions options) : base(options)
    {
        Database.EnsureCreated();
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Category> Categories { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>()
            .ToTable("Products");
        modelBuilder.Entity<Category>()
             .ToTable("Categories");
       
        modelBuilder.Entity<Category>()
            .HasData(
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Electronics"                   
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Office"
                },
                new Category
                {
                    Id = Guid.NewGuid(),
                    Name = "Home"
                }
            );
    }

}
