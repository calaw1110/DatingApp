using DatingApp.API.Data;
using DatingApp.API.Helper;
using DatingApp.API.Interfaces;
using DatingApp.API.Repositries;
using DatingApp.API.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
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
			// 加入Cloudinary
			services.Configure<CloudinaryHelper>(config.GetSection("CloudinarySettings"));
			services.AddScoped<IPhotoService, PhotoService>();

			// 加入 Repository
			services.AddScoped<IUserRepository, UserRepository>();
			services.AddScoped<ILikeRepository, LikesRepository>();
			services.AddScoped<IMessageRepository, MessageRepository>();

			// 加入 automapper
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


			// 加入 使用者活動時間紀錄
			services.AddScoped<LogUserActivityHelper>();

			return services;
		}
	}
}