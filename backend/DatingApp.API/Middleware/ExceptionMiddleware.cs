using DatingApp.API.Errors;
using Microsoft.AspNetCore.Http;
using System.Net;
using System.Text.Json;

namespace DatingApp.API.Middleware
{
	public class ExceptionMiddleware
	{
		private readonly RequestDelegate _next;
		private readonly ILogger<ExceptionMiddleware> _logger;
		private readonly IHostEnvironment _env;

		public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
		{
			this._next = next;
			this._logger = logger;
			this._env = env;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, ex.Message);
				context.Response.ContentType = "application/json";
				context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

				var response = _env.IsDevelopment()
					? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace?.ToString())
					: new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");

				//設定json key 使用駝峰式大小寫原則
				var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };


				// Origin response content
				// {
				//	 StatusCode = "...";
				//	 Message = "...";
				//	 Details = "...";
				// }


				var json = JsonSerializer.Serialize(response, options);

				// After Serialize
				// {
				//	 "statusCode": "...",
				//	 "message": "...",
				//	 "details":""
				// }

				await context.Response.WriteAsync(json);
			}
		}
	}
}
