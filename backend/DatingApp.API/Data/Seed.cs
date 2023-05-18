using DatingApp.API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DatingApp.API.Data
{
	public class Seed
	{
		public static async Task SeedUses(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)
		{
			if (await userManager.Users.AnyAsync()) return;

			var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

			var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

			var roles = new List<AppRole>()
			{
				new AppRole(){ Name ="Member"},
				new AppRole(){ Name ="Admin"},
				new AppRole(){ Name ="Moderator"},
			};

			foreach (var role in roles)
			{
				await roleManager.CreateAsync(role);
			}

			foreach (var user in users)
			{
				user.UserName = user.UserName.ToLower();

				await userManager.CreateAsync(user, "1234");
				await userManager.AddToRoleAsync(user, "Member");
			}

			var admin = new AppUser()
			{
				UserName = "admin",
			};
			await userManager.CreateAsync(admin, "1234");
			await userManager.AddToRolesAsync(admin, new[] { "Admin", "Moderator" });
		}
	}
}
