using DatingApp.API.Entities;
using Microsoft.EntityFrameworkCore;
namespace DatingApp.API.Data
{
    public class DatingAppData : DbContext
    {
        public DatingAppData(DbContextOptions options) : base(options)
        {
        }
        public DbSet<AppUser> Users { get; set; }
    }
}