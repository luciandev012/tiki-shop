using Microsoft.EntityFrameworkCore;
using tiki_shop.Models.Entity;

namespace tiki_shop.Models
{
    public class TikiDbContext : DbContext
    {
        public TikiDbContext(DbContextOptions<TikiDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Recharge> Recharges { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var connectionString = configuration.GetConnectionString("Connection");
            optionsBuilder.UseSqlServer(connectionString);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<Product>().HasKey(p => p.Id);
            
            builder.Entity<Category>().HasKey(c => c.Id);
            builder.Entity<Category>(e =>
            {
                e.Property(c => c.Id).HasMaxLength(100);
            });

            builder.Entity<SubCategory>().HasKey(s => s.Id);
            

            builder.Entity<ProductDetail>().HasKey(pd => pd.Id);
            builder.Entity<ProductDetail>(e =>
            {
                e.Property(pd => pd.Id).HasMaxLength(100);
                e.HasOne(pd => pd.Product).WithMany(p => p.ProductDetails).HasForeignKey(pd => pd.ProductId);
            });

            builder.Entity<Order>().HasKey(o => o.Id);
            builder.Entity<Order>(e =>
            {
                e.Property(o => o.Id).HasMaxLength(100);
            });

            builder.Entity<OrderDetail>().HasKey(od => od.Id);
            builder.Entity<OrderDetail>(e =>
            {
                e.Property(od => od.Id).HasMaxLength(100);
                e.HasOne(od => od.Order).WithMany(o => o.OrderDetails).HasForeignKey(od => od.OrderId);
            });

            builder.Entity<Recharge>().HasKey(r => r.Id);
            builder.Entity<Recharge>(e =>
            {
                e.Property(r => r.Id).HasMaxLength(100);
            });

            builder.Entity<Image>().HasKey(i => i.Id);
            builder.Entity<Image>(e =>
            {
                e.Property(i => i.Id).HasMaxLength(100);
                e.HasOne(i => i.Product).WithMany(p => p.Images).HasForeignKey(i => i.ProductId);
            });

            // Seeding data
            builder.Entity<Role>().HasData(new Role { Name = "Admin" }, new Role { Name = "User" });
            builder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid().ToString(),
                Address = "Hà Nội",
                Fullname = "Admin nè",
                Balance = 0,
                Email = "admin@email.com",
                Password = "$2a$10$MEhfF4w8ga3GGcJrMW7iWu6RG0A1kUqC0FM0R9BbJjSd3yKxgBLM2",
                PhoneNumber = "0123456789",
                Status = true
            });
            base.OnModelCreating(builder);
        }
    }
}
