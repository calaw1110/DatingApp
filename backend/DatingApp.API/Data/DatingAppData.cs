using DatingApp.API.Entities;
using Microsoft.EntityFrameworkCore;
namespace DatingApp.API.Data
{
    public class DatingAppDataContext : DbContext
    {
        public DatingAppDataContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
    }
}