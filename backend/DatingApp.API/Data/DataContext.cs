using DatingApp.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
	// 指定 Identity相關table pk 使用 int
	public class DataContext : IdentityDbContext<AppUser, AppRole, int, IdentityUserClaim<int>,
		AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>, IdentityUserToken<int>>
	{
		public DataContext(DbContextOptions options) : base(options)
		{
		}

		public DbSet<UserLike> Likes { get; set; }
		public DbSet<Message> Messages { get; set; }
		public DbSet<Group> Groups { get; set; }

		public DbSet<Connection> Connections { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<AppUser>()
				// 配置一對多關係：一個 AppUser 可以擁有多個 UserRoles
				.HasMany(ur => ur.UserRoles)
				// 設定 UserRoles 中的每個 UserRole 對象引用同一個 AppUser 對象
				.WithOne(u => u.User)
				// 設定外鍵：UserRoles 中的外鍵是 UserId
				.HasForeignKey(ur => ur.UserId)
				// 設定外鍵為必需的，不能為 null
				.IsRequired();


			modelBuilder.Entity<AppRole>()
				// 配置一對多關係：一個 AppRole 可以擁有多個 UserRoles
				.HasMany(ur => ur.UserRoles)
				// 設定 UserRoles 中的每個 UserRole 對象引用同一個 AppRole 對象
				.WithOne(u => u.Role)
				// 設定外鍵：UserRoles 中的外鍵是 RoleId
				.HasForeignKey(ur => ur.RoleId)
				// 設定外鍵為必需的，不能為 null
				.IsRequired();




			modelBuilder.Entity<UserLike>()
				// 設定複合主鍵：由 SourceUserId 和 TargetUserId 兩個屬性組成
				.HasKey(k => new { k.SourceUserId, k.TargetUserId });

			modelBuilder.Entity<UserLike>()
				// UserLike 類型中的 SourceUser 屬性引用一個 AppUser 對象
				.HasOne(s => s.SourceUser)
				// AppUser 類型中的 LikedUsers 屬性表示被 SourceUser 喜歡的多個 UserLike 對象
				.WithMany(l => l.LikedUsers)
				// 設定外鍵：UserLike 中的外鍵是 SourceUserId
				.HasForeignKey(fk => fk.SourceUserId)
				// 設定串聯刪除：當 SourceUser 被刪除時，相應的 UserLike 對象也會被刪除
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<UserLike>()
				// UserLike 類型中的 TargetUser 屬性引用一個 AppUser 對象
				.HasOne(s => s.TargetUser)
				// AppUser 類型中的 LikedByUsers 屬性表示喜歡 TargetUser 的多個 UserLike 對象
				.WithMany(l => l.LikedByUsers)
				// 設定外鍵：UserLike 中的外鍵是 TargetUserId
				.HasForeignKey(fk => fk.TargetUserId)
				// 設定串聯刪除：當 TargetUser 被刪除時，相應的 UserLike 對象也會被刪除
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Message>()
				// Message 類型中的 Recipient 屬性引用一個 AppUser 對象
				.HasOne(u => u.Recipient)
				// AppUser 類型中的 MessagesReceived 屬性表示接收到的多個 Message 對象
				.WithMany(m => m.MessagesReceived)
				// 禁用串聯刪除：刪除 Message 對象時不影響相應的 Recipient 對象
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Message>()
				// Message 類型中的 Sender 屬性引用一個 AppUser 對象
				.HasOne(u => u.Sender)
				// AppUser 類型中的 MessagesSent 屬性表示發送的多個 Message 對象
				.WithMany(m => m.MessagesSent)
				// 禁用串聯刪除：刪除 Message 對象時不影響相應的 Sender 對象
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}