namespace DatingApp.API.Helper
{
	/// <summary> 查詢參數 </summary>
	public class UserParams
	{
		/// <summary> 單頁資料最大顯示筆數 </summary>
		private const int MaxPageSize = 50;

		/// <summary> 目標分頁 </summary>
		public int PageNumber { get; set; } = 1;

		/// <summary> 預設顯示比數 </summary>
		private int _pageSize = 10;

		/// <summary> 顯示比數 </summary>
		public int PageSize
		{
			get { return _pageSize; }

			set
			{
				if (value > MaxPageSize)
				{
					_pageSize = MaxPageSize;
				}
				else
				{
					_pageSize = value;
				}
			}

			// 語法簡化
			//set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
		}

		/// <summary> 使用者名稱 </summary>
		public string CurrentUserName { get; set; }

		/// <summary> 性別 </summary>
		public string Gender { get; set; }

		/// <summary> 默認最小年齡 </summary>
		public int MinAge { get; set; } = 18;

		public int MaxAge { get; set; } = 100;

		public string OrderBy { get; set; } = "lastActive";
	}
}
