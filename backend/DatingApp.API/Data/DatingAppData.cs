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
		public DbSet<UserLike> Likes { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserLike>()
				.HasKey(k => new { k.SourceUserId, k.TargetUserId });

			modelBuilder.Entity<UserLike>()
				.HasOne(s => s.SourceUser)
				.WithMany(l => l.LikedUsers)
				.HasForeignKey(fk => fk.SourceUserId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<UserLike>()
				.HasOne(s => s.TargetUser)
				.WithMany(l => l.LikedByUsers)
				.HasForeignKey(fk => fk.TargetUserId)
				.OnDelete(DeleteBehavior.Cascade);
		}

	}
}