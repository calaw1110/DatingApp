using System.Security.Claims;

namespace DatingApp.API.Extensions
{
	public static class ClaimPrincipalExtensions
	{
		/// <summary>
		/// 取得使用者帳號
		/// </summary>
		/// <param name="user">當前已驗證授權的使用者</param>
		/// <returns></returns>
		public static string GetUsername(this ClaimsPrincipal user)
		{
			return user.FindFirst(ClaimTypes.Name)?.Value;
		}
		public static int GetUserId(this ClaimsPrincipal user)
		{
			return int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
		}
	}
}
