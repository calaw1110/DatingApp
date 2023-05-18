using DatingApp.API.Data;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Middleware;
using DatingApp.API.SignalR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// 加入 自訂Service集合
builder.Services.AddApplicationServices(builder.Configuration);

// 使用jwt驗證
builder.Services.AddIdentityService(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

// 註冊 Swagger UI 產生器
builder.Services.AddSwaggerGen(options =>
{
	// 定義 Request Header 要帶入 Authorization 以及 Token 的驗證資訊
	options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
	{
		Name = "Authorization",
		// Type使用SecuritySchemeType.Http，輸入Token時 不用打上 「Bearer 」
		Type = SecuritySchemeType.Http,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT驗證 請貼上TOKEN: {token}"
	});

	// 設定 "Bearer" 的 SecurityScheme
	options.AddSecurityRequirement(
		new OpenApiSecurityRequirement
		{
			{
				new OpenApiSecurityScheme
				{
					Reference = new OpenApiReference
					{
						Type = ReferenceType.SecurityScheme,
						Id="Bearer"
					}
				},
				new string[] {}
			}
		});
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}


// add error handler middleware
app.UseMiddleware<ExceptionMiddleware>();



// 因調整launchSetting.json 註解以下功能 
// app.UseHttpsRedirection();


// 調整CORS政策 允許 http://localhost:4200 任何請求
app.UseCors(builder => builder
						.AllowAnyHeader()
						.AllowAnyMethod()
						.AllowCredentials() // 是 SignalR 的一個選項，用於設定是否允許跨域請求攜帶身份驗證憑證。
						.WithOrigins("https://localhost:4200"));

// 先驗證
app.UseAuthentication();

// 後授權
app.UseAuthorization();


app.MapControllers();

// Map To Hub
app.MapHub<PresenceHub>("hubs/presence");
app.MapHub<MessageHub>("hubs/message");
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
	// 自動建立 使用者測試資料
	var context = services.GetRequiredService<DataContext>();
	var userManager = services.GetRequiredService<UserManager<AppUser>>();
	var roleManager = services.GetRequiredService<RoleManager<AppRole>>();
	await context.Database.MigrateAsync();
	await context.Database.ExecuteSqlRawAsync("DELETE FROM [Connections]");
	await Seed.SeedUses(userManager, roleManager);
}
catch (Exception ex)
{
	var logger = services.GetService<ILogger<Program>>();
	logger.LogError(ex, "An error occurred during migrations");
}

app.Run();
