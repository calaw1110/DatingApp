using DatingApp.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
	// ���w Identity����table pk �ϥ� int
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
				// �t�m�@��h���Y�G�@�� AppUser �i�H�֦��h�� UserRoles
				.HasMany(ur => ur.UserRoles)
				// �]�w UserRoles �����C�� UserRole ��H�ޥΦP�@�� AppUser ��H
				.WithOne(u => u.User)
				// �]�w�~��GUserRoles �����~��O UserId
				.HasForeignKey(ur => ur.UserId)
				// �]�w�~�䬰���ݪ��A���ର null
				.IsRequired();


			modelBuilder.Entity<AppRole>()
				// �t�m�@��h���Y�G�@�� AppRole �i�H�֦��h�� UserRoles
				.HasMany(ur => ur.UserRoles)
				// �]�w UserRoles �����C�� UserRole ��H�ޥΦP�@�� AppRole ��H
				.WithOne(u => u.Role)
				// �]�w�~��GUserRoles �����~��O RoleId
				.HasForeignKey(ur => ur.RoleId)
				// �]�w�~�䬰���ݪ��A���ର null
				.IsRequired();




			modelBuilder.Entity<UserLike>()
				// �]�w�ƦX�D��G�� SourceUserId �M TargetUserId ����ݩʲզ�
				.HasKey(k => new { k.SourceUserId, k.TargetUserId });

			modelBuilder.Entity<UserLike>()
				// UserLike �������� SourceUser �ݩʤޥΤ@�� AppUser ��H
				.HasOne(s => s.SourceUser)
				// AppUser �������� LikedUsers �ݩʪ�ܳQ SourceUser ���w���h�� UserLike ��H
				.WithMany(l => l.LikedUsers)
				// �]�w�~��GUserLike �����~��O SourceUserId
				.HasForeignKey(fk => fk.SourceUserId)
				// �]�w���p�R���G�� SourceUser �Q�R���ɡA������ UserLike ��H�]�|�Q�R��
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<UserLike>()
				// UserLike �������� TargetUser �ݩʤޥΤ@�� AppUser ��H
				.HasOne(s => s.TargetUser)
				// AppUser �������� LikedByUsers �ݩʪ�ܳ��w TargetUser ���h�� UserLike ��H
				.WithMany(l => l.LikedByUsers)
				// �]�w�~��GUserLike �����~��O TargetUserId
				.HasForeignKey(fk => fk.TargetUserId)
				// �]�w���p�R���G�� TargetUser �Q�R���ɡA������ UserLike ��H�]�|�Q�R��
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Message>()
				// Message �������� Recipient �ݩʤޥΤ@�� AppUser ��H
				.HasOne(u => u.Recipient)
				// AppUser �������� MessagesReceived �ݩʪ�ܱ����쪺�h�� Message ��H
				.WithMany(m => m.MessagesReceived)
				// �T�Φ��p�R���G�R�� Message ��H�ɤ��v�T������ Recipient ��H
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<Message>()
				// Message �������� Sender �ݩʤޥΤ@�� AppUser ��H
				.HasOne(u => u.Sender)
				// AppUser �������� MessagesSent �ݩʪ�ܵo�e���h�� Message ��H
				.WithMany(m => m.MessagesSent)
				// �T�Φ��p�R���G�R�� Message ��H�ɤ��v�T������ Sender ��H
				.OnDelete(DeleteBehavior.Restrict);

		}
	}
}