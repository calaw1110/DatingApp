namespace DatingApp.API.Helper
{
	/// <summary>
	/// 分頁資訊 類別
	/// </summary>
	public class PaginationHeader
	{
		/// <summary>
		/// 分頁資訊 建構式
		/// </summary>
		/// <param name="currentPage">目前分頁</param>
		/// <param name="itemsPerPage">目前分頁資訊比數</param>
		/// <param name="totalItems">資訊總比數</param>
		/// <param name="totalPages">分頁總數</param>
		public PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
		{
			CurrentPage = currentPage;
			ItemsPerPage = itemsPerPage;
			TotalItems = totalItems;
			TotalPages = totalPages;
		}

		/// <summary> 目前分頁 </summary>
		public int CurrentPage { get; set; }

		/// <summary> 目前分頁資訊比數 </summary>
		public int ItemsPerPage { get; set; }

		/// <summary> 資訊總比數 </summary>
		public int TotalItems { get; set; }

		/// <summary> 分頁總數 </summary>
		public int TotalPages { get; set; }
	}
}
