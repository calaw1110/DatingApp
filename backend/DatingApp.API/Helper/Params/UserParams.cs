namespace DatingApp.API.Helper.Params
{
    /// <summary> 查詢參數 </summary>
    public class UserParams : PaginationParams
    {

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
