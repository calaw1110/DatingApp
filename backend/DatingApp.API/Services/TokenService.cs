using DatingApp.API.Entities;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingApp.API.Services
{
	/// <summary> Token服務 </summary>
	public class TokenService : ITokenService
	{
		private readonly SymmetricSecurityKey _key;
		private readonly UserManager<AppUser> _userManager;

		public TokenService(IConfiguration config, UserManager<AppUser> userManager)
		{
			// 從appsetting中取得 TokenKey 值並將其轉換為 UTF-8 編碼的 byte 陣列
			// 建立一個加密金鑰 用於 JWT
			_key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
			this._userManager = userManager;
		}

		/// <summary>
		/// 建立 帳號登入 token
		/// </summary>
		/// <param name="user">通過驗證使用者</param>
		/// <returns></returns>
		public async Task<string> CreateToken(AppUser user)
		{
			// 建立一個宣告（Claim）的List
			var claims = new List<Claim>
			{
				// 在List中添加一個Claim，Claim類型為 JwtRegisteredClaimNames.NameId
				// 宣告的值是使用者的識別符（user.Id.ToString()）

				new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),

				// 在List中添加一個Claim，Claim類型為 JwtRegisteredClaimNames.UniqueName
				// 宣告的值是使用者的使用者名稱（user.UserName）

				new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
			};

			var roles = await _userManager.GetRolesAsync(user);

			claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

			// 建立一個簽署憑證（SigningCredentials）
			// 使用 _key 作為金鑰（_key 是一個 SecurityKey 物件），使用 HmacSha512Signature 簽署算法
			var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

			// 建立一個安全性憑證描述（SecurityTokenDescriptor）
			// 設定主題（Subject）為包含上述宣告的宣告身份（ClaimsIdentity）
			// 設定到期時間（Expires）為現在時間加上 7 天
			// 設定簽署憑證（SigningCredentials）為上述建立的簽署憑證（creds）
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(claims),
				Expires = DateTime.Now.AddDays(7),
				SigningCredentials = creds
			};

			var tokenHandler = new JwtSecurityTokenHandler();

			// 使用 tokenDescriptor 來建立 JWT
			var token = tokenHandler.CreateToken(tokenDescriptor);

			// 回傳token
			return tokenHandler.WriteToken(token);
		}
	}
}