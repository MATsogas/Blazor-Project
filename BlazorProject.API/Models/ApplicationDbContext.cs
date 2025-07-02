using BlazorApp.Models;
using Microsoft.EntityFrameworkCore;

namespace BlazorApp.API.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Customer> Customers { get; set; }
    }
}