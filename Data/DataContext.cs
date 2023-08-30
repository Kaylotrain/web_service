using Microsoft.EntityFrameworkCore;
using web_service.Entities;

namespace web_service.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<AppUser> Users { get; set; }
        
    }
}