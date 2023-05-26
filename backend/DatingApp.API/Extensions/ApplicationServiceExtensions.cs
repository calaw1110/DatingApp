using DatingApp.API.Data;
using DatingApp.API.Helper;
using DatingApp.API.Interfaces;
using DatingApp.API.Repositries;
using DatingApp.API.Services;
using DatingApp.API.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Extensions
{
	public static class ApplicationServiceExtensions
	{
		public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
		{
			// 加入資料庫連線設定
			//services.AddDbContext<DataContext>(option =>
			//{
			//	//option.UseSqlite(config.GetConnectionString("SqliteConnection"));
			//	option.UseNpgsql(config.GetConnectionString("PgsqlConneciton"));
			//});

			// CORS

			services.AddCors();


			// 加入Cloudinary
			services.Configure<CloudinaryHelper>(config.GetSection("CloudinarySettings"));

			#region Scoped - 同一個 Request 中，都是同樣的實例

			// 加入自訂Service
			services.AddScoped<ITokenService, TokenService>();
			services.AddScoped<IPhotoService, PhotoService>();

			// 加入 Repository
			//services.AddScoped<IUserRepository, UserRepository>();
			//services.AddScoped<ILikeRepository, LikesRepository>();
			//services.AddScoped<IMessageRepository, MessageRepository>();

			// 將個別注入的Repository服務 整合成Unit Of Work 一次注入
			services.AddScoped<IUnitOfWork, UnitOfWork>();


			// 加入 使用者活動時間紀錄
			services.AddScoped<LogUserActivityHelper>();

			#endregion

			// 加入 automapper
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


			// 加入 SignalR 服務
			services.AddSignalR();
			// Singleton - 應用程式 執行時期 只有一個實體
			services.AddSingleton<PresenceTracker>();
			return services;
		}
	}
}