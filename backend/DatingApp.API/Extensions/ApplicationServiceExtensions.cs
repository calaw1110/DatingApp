using DatingApp.API.Data;
using DatingApp.API.Interfaces;
using DatingApp.API.Repositries;
using DatingApp.API.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services,IConfiguration config)
		{

			// 加入資料庫連線設定
			services.AddDbContext<DatingAppDataContext>(option =>
			{
				option.UseSqlite(config.GetConnectionString("DefaultConnection"));
			});

			// CORS

			services.AddCors();

			// 加入自訂Service 

			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IUserRepository,UserRepository>();

			// 加入automapper
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

			return services;
		}
	}
}
