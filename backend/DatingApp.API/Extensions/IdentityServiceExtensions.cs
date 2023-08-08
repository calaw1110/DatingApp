using DatingApp.API.Data;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DatingApp.API.Extensions
{
	/// <summary>
	/// 擴充方法類別，用於註冊身份驗證服務。
	/// </summary>
	public static class IdentityServiceExtensions
	{
		/// <summary>
		/// 將身份驗證服務添加到服務集合中。
		/// </summary>
		/// <param name="services">服務集合。</param>
		/// <param name="config">應用程式配置設定。</param>
		/// <returns>修改後的服務集合。</returns>
		public static IServiceCollection AddIdentityService(this IServiceCollection services, IConfiguration config)
		{
			services.AddIdentityCore<AppUser>(option =>
			{
				option.Password.RequireNonAlphanumeric = false;
				option.Password.RequiredLength = 4;
				option.Password.RequireLowercase = false;
				option.Password.RequireUppercase = false;
				option.Password.RequireDigit = false;
			})
			.AddRoles<AppRole>()
			.AddRoleManager<RoleManager<AppRole>>()
			.AddEntityFrameworkStores<DataContext>();

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				.AddJwtBearer(options =>
				{
					options.TokenValidationParameters = new TokenValidationParameters
					{
						ValidateIssuerSigningKey = true,
						IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])),
						ValidateIssuer = false,
						ValidateAudience = false
					};

					options.Events = new JwtBearerEvents()
					{
						OnMessageReceived = context =>
						{
							// Client端 SignalR 傳送資訊時所使用的Token
							var accessToken = context.Request.Query["access_token"];

							var path = context.HttpContext.Request.Path;
							if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
							{
								context.Token = accessToken;
							}
							return Task.CompletedTask;
						}
					};
				});

			// 加入自定義的授權政策
			services.AddAuthorization(options =>
			{
				// [Authorize(Policy = "RequireAdminRole")]
				// endpoint 有此屬性時，呼叫的使用者角色需有 Admin
				options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));

				// [Authorize(Policy = "ModeratePhotoRole")]
				// endpoint 有此屬性時，呼叫的使用者角色需有 Admin 或 Moderator
				options.AddPolicy("ModeratePhotoRole", policy => policy.RequireRole("Admin", "Moderator"));
			});

			return services;
		}
	}
}