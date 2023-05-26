using DatingApp.API.Errors;
using System.Net;
using System.Text.Json;

namespace DatingApp.API.Middleware
{
	/// <summary>
	/// 自訂的例外處理中介軟體，用於捕獲並處理應用程式中的例外狀況。
	/// </summary>
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		/// <summary> 初始化 <see cref="ExceptionMiddleware"/> 類別的新執行個體 </summary>
		/// <param name="next"> 下一個中介軟體（Middleware） </param>
		/// <param name="logger"> 用於記錄例外狀況的日誌 </param>
		/// <param name="env"> 應用程式執行環境 </param>
		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
		{
			this._next = next;
			this._logger = logger;
			this._env = env;
		}

		/// <summary> 執行例外處理中介軟體的主要方法 </summary>
		/// <param name="context"> HTTP Context </param>
		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				// 執行下一個中介軟體（Middleware）
				await _next(context);
			}
			catch (Exception ex)
			{
				// 記錄例外狀況到日誌
				_logger.LogError(ex, ex.Message);

				// 設定回應的內容類型為 "application/json"
				context.Response.ContentType = "application/json";

				// 設定回應的狀態碼為 500 (Internal Server Error)
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				// 建立一個 ApiException 物件，根據應用程式執行環境決定訊息內容
				var response = _env.IsDevelopment()
					? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
					: new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

				// 設定 JSON 序列化選項，使用駝峰式大小寫原則
				var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

				// 將 ApiException 物件序列化為 JSON 字串
				var json = JsonSerializer.Serialize(response, options);

				// 將 JSON 字串寫入回應
				await context.Response.WriteAsync(json);
			}
		}
	}
}
