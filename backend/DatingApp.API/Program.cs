using DatingApp.API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// 加入資料庫連線設定
builder.Services.AddDbContext<DatingAppData>(option =>
{
    option.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnetion"));
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// 因調整launchSetting.json 註解以下功能 
// app.UseHttpsRedirection();

// 暫無使用授權相關
// app.UseAuthorization();

app.MapControllers();

app.Run();
