using DatingApp.API.Extensions;
using DatingApp.API.Helper.Params;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DatingApp.API.Helper
{
	/// <summary>
	/// 用於記錄使用者活動時間的幫助程式類別。
	/// 實作了 <see cref="IAsyncActionFilter"/> 介面，以在動作方法執行前後進行相應處理。
	/// </summary>
	public class LogUserActivityHelper : IAsyncActionFilter
	{
		private readonly ILogger<LogUserActivityHelper> _logger;

		/// <summary>
		/// 初始化 <see cref="LogUserActivityHelper"/> 類別的新執行個體。
		/// </summary>
		/// <param name="logger">用於記錄日誌的日誌記錄器。</param>
		public LogUserActivityHelper(ILogger<LogUserActivityHelper> logger)
		{
			_logger = logger;
		}

		/// <summary> 在動作方法執行前後進行相應處理，持續更新使用者活動時間並進行日誌記錄 </summary>
		/// <param name="context"> 動作執行Context </param>
		/// <param name="next"> 下一個動作執行委派 </param>
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			// 執行下一個動作方法
			var resultContext = await next();

			// 檢查使用者是否已驗證，若未驗證則不進行處理
			if (!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

			// 取得使用者 Repository 服務
			var uow = resultContext.HttpContext.RequestServices.GetRequiredService<IUnitOfWork>();

			// 取得使用者id
			var userId = resultContext.HttpContext.User.GetUserId();

			// 取得使用者
			var user = await uow.UserRepository.GetUserByIdAsync(userId);

			// 更新使用者的最後活動時間為當前時間（UTC 格式）
			user.LastActive = DateTime.UtcNow;

			// 取得當前執行的動作方法的相關資訊
			var controllerName = context.RouteData.Values["controller"];
			var actionName = context.RouteData.Values["action"];

			// 取得請求參數
			var requestParams = context.ActionArguments;

			// 在動作方法執行後進行日誌記錄
			_logger.LogInformation($"帳號：{user.UserName} 的使用者於 {user.LastActive.ToLocalTime()} 執行 /api/{controllerName}/{actionName}");

			// 如果存在請求參數，則進行參數的日誌記錄
			if (requestParams != null)
			{
				foreach (var param in requestParams)
				{
					if (param.Value.GetType().Name.ToString().ToLower() != "userparams")
					{
						_logger.LogInformation($"帳號：{user.UserName} 的使用者 請求的參數 -> {param.Key}: {param.Value}");
					}
					else
					{
						Type paramDetails = typeof(UserParams);
						foreach (var paramDetail in paramDetails.GetProperties())
						{
							_logger.LogInformation($"帳號：{user.UserName} 的使用者 請求的參數 -> {paramDetail.Name}: {paramDetail.GetValue(param.Value)}");
						}
					}
				}
			}

			// 儲存更新後的使用者資料
			await uow.Complete();
		}
	}
}