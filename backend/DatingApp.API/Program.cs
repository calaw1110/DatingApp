using DatingApp.API.Data;
using DatingApp.API.Extensions;
using DatingApp.API.Interfaces;
using DatingApp.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

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
		Type = SecuritySchemeType.ApiKey,
		Scheme = "Bearer",
		BearerFormat = "JWT",
		In = ParameterLocation.Header,
		Description = "JWT Authorization"
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
// 因調整launchSetting.json 註解以下功能 
// app.UseHttpsRedirection();


// 調整CORS政策 允許 http://localhost:4200 任何請求
app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200"));

// 先驗證
app.UseAuthentication();

// 後授權
app.UseAuthorization();


app.MapControllers();

app.Run();
