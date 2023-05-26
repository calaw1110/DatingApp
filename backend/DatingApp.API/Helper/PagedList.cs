using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Helper
{
	/// <summary>
	/// 分頁資訊類別
	/// </summary>
	/// <typeparam name="T">有需要做分頁查詢的類別</typeparam>
	public class PagedList<T> : List<T>
	{
		/// <summary>
		/// 分頁建構
		/// </summary>
		/// <param name="items">取得的資料</param>
		/// <param name="count">資料總比數</param>
		/// <param name="pageNumber">目標頁數</param>
		/// <param name="pageSize">頁面資料顯示比數</param>
		public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
		{
			CurrentPage = pageNumber;
			TotalPages = (int)Math.Ceiling(count / (double)pageSize);
			PageSize = pageSize;
			TotalCount = count;
			AddRange(items);
		}

		/// <summary> 目前的分頁 </summary>
		public int CurrentPage { get; set; }

		/// <summary> 總頁數 </summary>
		public int TotalPages { get; set; }

		/// <summary> 頁面資料顯示筆數 </summary>
		public int PageSize { get; set; }

		/// <summary> 資料總比數 </summary>
		public int TotalCount { get; set; }

		/// <summary>
		/// 建立頁面列表、當前頁面資訊
		/// </summary>
		/// <param name="source"> 資料查詢結果 </param>
		/// <param name="pageNumber"> 目標分頁數 </param>
		/// <param name="pageSize"> 頁面資料顯示比數 </param>
		/// <returns></returns>
		public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
		{
			var count = await source.CountAsync();
			var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
			return new PagedList<T>(items, count, pageNumber, pageSize);
		}
	}
}
