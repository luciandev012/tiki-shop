using Microsoft.EntityFrameworkCore;
using tiki_shop.Models.Entity;

namespace tiki_shop.Models
{
    public class TikiDbContext : DbContext
    {
        public TikiDbContext(DbContextOptions<TikiDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

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
            builder.Entity<Role>().HasData(new Role { Id = 1, Name = "Admin" }, new Role { Id = 2, Name = "User" });

            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<User>().HasOne<Role>(u => u.Role).WithMany(r => r.Users).HasForeignKey(u => u.RoleId);
            builder.Entity<User>().HasData(new User
            {
                Id = Guid.NewGuid().ToString(),
                Address = "Hà Nội",
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
