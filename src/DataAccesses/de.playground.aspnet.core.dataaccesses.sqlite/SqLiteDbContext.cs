using de.playground.aspnet.core.contracts.pocos;

using Microsoft.EntityFrameworkCore;

namespace de.playground.aspnet.core.dataaccesses.sqlite
{
    public class SqLiteDbContext : DbContext
    {
        public SqLiteDbContext(DbContextOptions<SqLiteDbContext> options) : base(options)
        {
        }

        public DbSet<CustomerPoco> Customers { get; set; }
        public DbSet<ProductPoco> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerPoco>().ToTable("Customers");

            modelBuilder.Entity<ProductPoco>()
                .ToTable("Products")
                .HasOne<CustomerPoco>()
                .WithMany()
                .HasForeignKey(productPoco => productPoco.CustomerId)
                .HasConstraintName("FK_Customer_Product")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
