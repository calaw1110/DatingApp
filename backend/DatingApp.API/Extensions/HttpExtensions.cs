using DatingApp.API.Helper;
using System.Text.Json;

namespace DatingApp.API.Extensions
{
	/// <summary>
	/// 自訂 HttpResponse Extensions
	/// </summary>
	public static class HttpExtensions
	{
		/// <summary> Response Header 加入 資料查詢分頁資訊 </summary>
		/// <param name="response"> 當下要回傳的response </param>
		/// <param name="header"> 分頁資料 </param>
		public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
		{
			var jsonOptions = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };

			// HttpResponse header 加入 分頁資訊
			response.Headers.Add("Pagination", JsonSerializer.Serialize(header, jsonOptions));

			// 使用Access-Control-Expose-Headers 公開自定義 Headers
			response.Headers.Add("Access-Control-Expose-Headers", "Pagination");


		}
	}
}
