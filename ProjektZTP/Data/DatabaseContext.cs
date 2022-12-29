using Microsoft.EntityFrameworkCore;
using ProjektZTP.Models;

namespace ProjektZTP.Data
{
    public class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ProductOrder> ProductsOrders { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //many to many
            modelBuilder.Entity<ProductOrder>().HasKey(po => new { po.ProductId, po.OrderId });

            modelBuilder.Entity<ProductOrder>()
                .HasOne<Product>(p => p.Product)
                .WithMany(o => o.ProductOrder)
                .HasForeignKey(p => p.ProductId);

            modelBuilder.Entity<ProductOrder>()
                .HasOne<Order>(o => o.Order)
                .WithMany(p => p.ProductOrder)
                .HasForeignKey(o => o.OrderId);

            //one to many 
            modelBuilder.Entity<Order>()
                .HasOne<User>(u => u.User)
                .WithMany(o => o.Orders)
                .HasForeignKey(u => u.UserId);

            //data annotations
            modelBuilder.Entity<User>(u =>
            {
                u.Property(p => p.Login).IsRequired();
                u.Property(p => p.Login).HasMaxLength(15);
                u.Property(p => p.Password).IsRequired();
                u.Property(p => p.Password).HasMaxLength(15);
                u.Property(p => p.Email).IsRequired();
                u.Property(p => p.Email).HasMaxLength(20);
                u.Property(p => p.FirstName).IsRequired();
                u.Property(p => p.FirstName).HasMaxLength(15);
                u.Property(p => p.LastName).IsRequired();
                u.Property(p => p.LastName).HasMaxLength(15);
            });

            modelBuilder.Entity<Product>(p =>
            {
                p.Property(prop => prop.Name).IsRequired();
                p.Property(prop => prop.Name).HasMaxLength(15);
                p.Property(prop => prop.Price).IsRequired();
                p.Property(prop => prop.Amount).IsRequired();
                p.Property(prop => prop.Vat).IsRequired();
            });

            modelBuilder.Entity<Order>(o =>
            {
                o.Property(p => p.Customer).IsRequired();
                o.Property(p => p.Customer).HasMaxLength(15);
                o.Property(p => p.Address).IsRequired();
                o.Property(p => p.Address).HasMaxLength(20);
            });
        }
    }
}
