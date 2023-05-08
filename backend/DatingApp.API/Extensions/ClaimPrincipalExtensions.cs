using System.Security.Claims;

namespace DatingApp.API.Extensions
{
	public static class ClaimPrincipalExtensions
	{
		public static string GetUsername(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
		}
	}
}
