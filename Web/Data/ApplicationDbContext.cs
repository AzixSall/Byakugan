using Microsoft.EntityFrameworkCore;
using Shared.Models;

namespace Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
       : base(options)
        {
        }

        public DbSet<IpDetails> IpDetails { get; set; }
    }
}
