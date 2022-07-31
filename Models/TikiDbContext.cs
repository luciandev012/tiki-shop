using Microsoft.EntityFrameworkCore;
using tiki_shop.Models.Entity;

namespace tiki_shop.Models
{
    public class TikiDbContext : DbContext
    {
        public TikiDbContext(DbContextOptions<TikiDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductDetail> ProductDetails { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Recharge> Recharges { get; set; }

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
            builder.Entity<Role>().HasKey(r => r.Id);
            

            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<User>().HasOne(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);

            builder.Entity<Product>().HasKey(p => p.Id);
            builder.Entity<Product>(e =>
            {
                e.Property(p => p.Id).HasMaxLength(100);
                e.HasOne(p => p.SubCategory).WithMany(s => s.Products).HasForeignKey(p => p.SubCategoryId);
            });

            builder.Entity<Category>().HasKey(c => c.Id);
            builder.Entity<Category>(e =>
            {
                e.Property(c => c.Id).HasMaxLength(100);
            });

            builder.Entity<SubCategory>().HasKey(s => s.Id);
            builder.Entity<SubCategory>(e =>
            {
                e.Property(s => s.Id).HasMaxLength(100);
                e.HasOne(s => s.Category).WithMany(c => c.SubCategories).HasForeignKey(s => s.CategoryId);
            });

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
                e.HasOne(r => r.User).WithMany(u => u.Recharges).HasForeignKey(r => r.UserId);
            });

            builder.Entity<Image>().HasKey(i => i.Id);
            builder.Entity<Image>(e =>
            {
                e.Property(i => i.Id).HasMaxLength(100);
                e.HasOne(i => i.Product).WithMany(p => p.Images).HasForeignKey(i => i.ProductId);
            });

            // Seeding data
            builder.Entity<Role>().HasData(new Role { Id = 1, Name = "Admin" }, new Role { Id = 2, Name = "User" });
            builder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid().ToString(),
                Address = "Hà Nội",
                Fullname = "Admin nè",
                Balance = 0,
                Commission = 0,
                Email = "admin@email.com",
                Password = "$2a$10$MEhfF4w8ga3GGcJrMW7iWu6RG0A1kUqC0FM0R9BbJjSd3yKxgBLM2",
                PhoneNumber = "0123456789",
                RoleId = 1,
                Status = true
            });
            
            

            base.OnModelCreating(builder);
        }
    }
}
