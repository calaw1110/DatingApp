using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Helper;

namespace DatingApp.API.Interfaces
{
	public interface ILikeRepository
	{
		Task<UserLike> GetUserLike(int sourceUserId, int targetUserId);
		Task<AppUser> GetUserWithLikes(int userId);

		Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams);
	}
}