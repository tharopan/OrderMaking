using OrderMaking.Models;
using System.Data.Entity;

namespace OrderMaking.Data
{
    public class OrderMakingContext : DbContext
    {
        public OrderMakingContext()
            : base("DefaultConnection") { }

        public DbSet<DeprecatedProduct> DeprecatedProducts { get; set; }

        public DbSet<DeprecatedDepartment> DeprecatedDepartments { get; set; }

        public DbSet<DeprecatedCategory> DeprecatedCategories { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<ShoppingCart> ShoppingCarts { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<DeprecatedProduct>().HasKey<int>(s => s.StudentKey);
            // configures one-to-many relationship
            //modelBuilder.Entity<DeprecatedProduct>()
            //    .HasOptional(s => s.Department); // Mark Address property optional in Student entity

        }
    }
}
