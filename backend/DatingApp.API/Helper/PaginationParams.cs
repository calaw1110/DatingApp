namespace DatingApp.API.Helper
{
	public class PaginationParams
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
	}
}
