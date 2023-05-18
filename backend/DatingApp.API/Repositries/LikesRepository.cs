using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Helper;
using DatingApp.API.Helper.Params;
using DatingApp.API.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Repositries
{
    public class LikesRepository : ILikeRepository
	{
		private readonly DataContext _context;

		public LikesRepository(DataContext context)
		{
			this._context = context;
		}

		public async Task<UserLike> GetUserLike(int sourceUserId, int tartgetUserId)
		{
			return await _context.Likes.FindAsync(sourceUserId, tartgetUserId);
		}

		public async Task<PagedList<LikeDto>> GetUserLikes(LikeParams likeParams)
		{
			var users = _context.Users.OrderBy(u => u.UserName).AsQueryable();

			var likes = _context.Likes.AsQueryable();

			if (likeParams.Predicate == "liked")
			{
				likes = likes.Where(like => like.SourceUserId == likeParams.UserId);
				users = likes.Select(like => like.TargetUser);
			}

			if (likeParams.Predicate == "likedBy")
			{
				likes = likes.Where(like => like.TargetUserId == likeParams.UserId);
				users = likes.Select(like => like.SourceUser);
			}

			var likedUser = users.Select(user => new LikeDto
			{
				UserName = user.UserName,
				KnownAs = user.KnownAs,
				Age = user.DateOfBirth.CalculateAge(),
				PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain).Url,
				City = user.City,
				Id = user.Id,
			});

			return await PagedList<LikeDto>.CreateAsync(likedUser, likeParams.PageNumber, likeParams.PageSize);
		}


		public async Task<AppUser> GetUserWithLikes(int userId)
		{
			return await _context.Users
					.Include(x => x.LikedUsers)
					.FirstOrDefaultAsync(x => x.Id == userId);
		}

		//public async Task<LikeDto> RemoveLikes(int userId, int wannaRemoveLikeUserId)
		//{
		//	var remove = _context.Likes.Where(w => w.SourceUserId == userId && w.TargetUserId == wannaRemoveLikeUserId);
		//	_context.Remove(remove);
		//	_context.SaveChanges();
		//	return _context.Likes.AsNoTracking().ToListAsync();
		//}
	}
}