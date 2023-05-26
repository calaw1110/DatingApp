namespace DatingApp.API.Errors
{
	/// <summary>
	///  API錯誤訊息 類別
	/// </summary>
	public class ApiException
	{
		/// <summary>
		/// 初始化 <see cref="ApiException"/> 類別的新執行個體。
		/// </summary>
		/// <param name="statusCode">錯誤的狀態碼。</param>
		/// <param name="message">錯誤的主要訊息。</param>
		/// <param name="details">錯誤的詳細資訊。</param>
		public ApiException(int statusCode, string message, string details)
		{
			StatusCode = statusCode;
			Message = message;
			Details = details;
		}

		/// <summary> HTTP 狀態碼 </summary>
		public int StatusCode { get; set; }

		/// <summary> 錯誤主要訊息 </summary>
		public string Message { get; set; }

		/// <summary> 錯誤詳細資訊 </summary>
		public string Details { get; set; }
	}
}
