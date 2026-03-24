using ConversationalAI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConversationalAI;

public class AppDbContext : DbContext
{
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Sale> Sales { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartItem> CartItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite("Data Source=app.db");

    
    // --- Seed Data for Example ---
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // --- Cascade delete config ---
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Product)
            .WithMany(p => p.Sales)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Category)
            .WithMany(c => c.Sales)
            .HasForeignKey(s => s.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        // --- Categories ---
        modelBuilder.Entity<Category>().HasData(
            new Category { Id = 1, Title = "Electronics",  Description = "Gadgets, devices, and accessories" },
            new Category { Id = 2, Title = "Clothing",     Description = "Apparel for all ages and styles" },
            new Category { Id = 3, Title = "Home & Garden", Description = "Everything for your living space" }
        );

        // --- Products (4 per category) ---
        modelBuilder.Entity<Product>().HasData(
            // Electronics
            new Product { Id = 1,  Title = "Wireless Headphones",  Description = "Over-ear noise cancelling headphones", Quantity = 40,  Price = 89.99m,  CategoryId = 1 },
            new Product { Id = 2,  Title = "Mechanical Keyboard",  Description = "Tenkeyless with brown switches",        Quantity = 25,  Price = 119.99m, CategoryId = 1 },
            new Product { Id = 3,  Title = "USB-C Hub",            Description = "7-in-1 multiport adapter",             Quantity = 60,  Price = 34.99m,  CategoryId = 1 },
            new Product { Id = 4,  Title = "Webcam 1080p",         Description = "HD webcam with built-in mic",          Quantity = 35,  Price = 59.99m,  CategoryId = 1 },
            // Clothing
            new Product { Id = 5,  Title = "Classic White Tee",    Description = "100% organic cotton t-shirt",          Quantity = 100, Price = 19.99m,  CategoryId = 2 },
            new Product { Id = 6,  Title = "Slim Chinos",          Description = "Stretch chino trousers",               Quantity = 50,  Price = 44.99m,  CategoryId = 2 },
            new Product { Id = 7,  Title = "Puffer Jacket",        Description = "Lightweight packable puffer",          Quantity = 30,  Price = 89.99m,  CategoryId = 2 },
            new Product { Id = 8,  Title = "Running Socks 3-Pack", Description = "Moisture-wicking ankle socks",         Quantity = 120, Price = 12.99m,  CategoryId = 2 },
            // Home & Garden
            new Product { Id = 9,  Title = "Ceramic Plant Pot",    Description = "Minimalist pot with drainage hole",    Quantity = 75,  Price = 14.99m,  CategoryId = 3 },
            new Product { Id = 10, Title = "Bamboo Cutting Board", Description = "Extra-large with juice groove",        Quantity = 45,  Price = 24.99m,  CategoryId = 3 },
            new Product { Id = 11, Title = "LED Desk Lamp",        Description = "Adjustable arm with USB charging",     Quantity = 55,  Price = 39.99m,  CategoryId = 3 },
            new Product { Id = 12, Title = "Scented Candle Set",   Description = "Set of 3 soy wax candles",            Quantity = 80,  Price = 29.99m,  CategoryId = 3 }
        );

        // --- Sales (1 product-level, 1 category-level) ---
        modelBuilder.Entity<Sale>().HasData(
            new Sale
            {
                Id = 1,
                Percentage = 20m,
                StartDate = new DateTime(2026, 3, 1,  0, 0, 0, DateTimeKind.Utc),
                EndDate   = new DateTime(2026, 3, 31, 0, 0, 0, DateTimeKind.Utc),
                ProductId  = 1,   // 20% off Wireless Headphones
                CategoryId = null
            },
            new Sale
            {
                Id = 2,
                Percentage = 15m,
                StartDate = new DateTime(2026, 3, 10, 0, 0, 0, DateTimeKind.Utc),
                EndDate   = new DateTime(2026, 4, 10, 0, 0, 0, DateTimeKind.Utc),
                ProductId  = null,
                CategoryId = 2    // 15% off all Clothing
            }
        );

        // --- Carts ---
        modelBuilder.Entity<Cart>().HasData(
            new Cart { Id = 1, CreatedAt = new DateTime(2026, 3, 14, 10, 0, 0, DateTimeKind.Utc) },
            new Cart { Id = 2, CreatedAt = new DateTime(2026, 3, 15, 14, 0, 0, DateTimeKind.Utc) }
        );

        // --- Cart Items ---
        modelBuilder.Entity<CartItem>().HasData(
            // Cart 1: a headphone + a tee
            new CartItem { Id = 1, CartId = 1, ProductId = 1, Quantity = 1 },
            new CartItem { Id = 2, CartId = 1, ProductId = 5, Quantity = 2 },
            // Cart 2: keyboard + chinos + candle set
            new CartItem { Id = 3, CartId = 2, ProductId = 2, Quantity = 1 },
            new CartItem { Id = 4, CartId = 2, ProductId = 6, Quantity = 1 },
            new CartItem { Id = 5, CartId = 2, ProductId = 12, Quantity = 3 }
        );
    }
}