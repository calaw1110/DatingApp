using Microsoft.AspNetCore.Identity;

namespace DatingApp.API.Entities
{
	public class AppUser : IdentityUser<int>
	{
		// DateOnly .net6才開使有 -> 單純針對某日
		public DateOnly DateOfBirth { get; set; }

		public string KnownAs { get; set; }

		// 不同時區的使用者
		public DateTime Created { get; set; } = DateTime.UtcNow;

		public DateTime LastActive { get; set; } = DateTime.UtcNow;

		public string Gender { get; set; }

		public string Introduction { get; set; }

		public string LookingFor { get; set; }

		public string Interests { get; set; }

		public string City { get; set; }

		public string Country { get; set; }

		public List<Photo> Photos { get; set; } = new();

		public List<UserLike> LikedByUsers { get; set; }

		public List<UserLike> LikedUsers { get; set; }

		public List<Message> MessagesSent { get; set; }

		public List<Message> MessagesReceived { get; set; }

		public ICollection<AppUserRole> UserRoles { get; set; }
	}
}