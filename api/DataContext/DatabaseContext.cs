using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.DataContext;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // base.OnModelCreating(modelBuilder);
        var apple = new Brand{Id = 1, BrandName = "Apple"};
        var acer = new Brand{Id = 2, BrandName = "Acer"};
        var dell = new Brand{Id = 3, BrandName = "Dell"};
        var hp = new Brand{Id = 4, BrandName = "HP"};
        var asus = new Brand{Id = 5, BrandName = "Asus"};
        var microsoft = new Brand{Id = 6, BrandName = "Microsoft"};
        modelBuilder.Entity<Brand>().HasData(apple, acer, dell, hp, asus, microsoft
        );

        modelBuilder.Entity<Category>().HasData(
            new Category{Id = 1, CategoryName = "Computers", Brands = new List<Brand>{}},
            new Category{Id = 2, CategoryName = "Laptops"},
            new Category{Id = 3, CategoryName = "Tablets"},
            new Category{Id = 4, CategoryName = "Headphones & Speakers"},
            new Category{Id = 5, CategoryName = "Cellphones"}
        );

        modelBuilder.Entity<Specification>().HasData(
            new Specification{Id = 1, Spec="Colour"},
            new Specification{Id = 2, Spec="Form factor"},
            new Specification{Id = 3, Spec="Wireless communication technology"},
            new Specification{Id = 4, Spec="Special feature"},
            new Specification{Id = 5, Spec="Noise control"},
            new Specification{Id = 6, Spec="Headphone jack"},
            new Specification{Id = 7, Spec="Ear placement"},
            new Specification{Id = 8, Spec="Model name"},
            new Specification{Id = 9, Spec="Screen size"},
            new Specification{Id = 10, Spec="Hard disk size"},
            new Specification{Id = 11, Spec="RAM memory installed size"},
            new Specification{Id = 12, Spec="Operating system"},
            new Specification{Id = 13, Spec="Graphics card description"},
            new Specification{Id = 14, Spec="Maximum display resolution"},
            new Specification{Id = 15, Spec="CPU speed"},
            new Specification{Id = 16, Spec="Shape"}
        );
    }

    public DbSet<Brand> Brands { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Specification> Specifications { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<SpecificationItem> SpecificationItems { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }
}
