namespace Shop.Web.Data
{
    using Microsoft.EntityFrameworkCore;
    using Entities;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

    public class DataContext : IdentityDbContext<User>
    {
        public DbSet<Product> Products { get; set; }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }
    }
}