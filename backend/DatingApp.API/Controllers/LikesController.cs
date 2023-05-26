using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Helper;
using DatingApp.API.Helper.Params;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
	// /api/Likes/...
	public class LikesController : BaseApiController
	{
		private readonly IUnitOfWork _uow;

		public LikesController(IUnitOfWork uow)
		{
			this._uow = uow;
		}

		[HttpPost("{username}")]
		public async Task<ActionResult> AddLike(string username)
		{
			var sourceUserId = User.GetUserId();

			var likeUser = await _uow.UserRepository.GetUserByUsernameAsync(username);

			var sourceUser = await _uow.LikeRepository.GetUserWithLikes(sourceUserId);

			if (likeUser == null) return NotFound();

			if (sourceUser.UserName == username) return BadRequest("You caanot like yourself");

			var userLike = await _uow.LikeRepository.GetUserLike(sourceUserId, likeUser.Id);

			if (userLike != null) return BadRequest("You already like this user");

			userLike = new UserLike
			{
				SourceUserId = sourceUserId,
				TargetUserId = likeUser.Id,
			};
			sourceUser.LikedUsers.Add(userLike);

			if (await _uow.Complete()) return Ok();

			return BadRequest("Failed to like user");

		}

		[HttpGet]
		public async Task<ActionResult<PagedList<LikeDto>>> GetUserLikes([FromQuery] LikeParams likeParams)
		{
			likeParams.UserId = User.GetUserId();

			if (string.IsNullOrEmpty(likeParams.Predicate)) likeParams.Predicate = "liked";

			var users = await _uow.LikeRepository.GetUserLikes(likeParams);

			Response.AddPaginationHeader(
				new PaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages)
				);

			return Ok(users);
		}

		//[HttpDelete]
		//public async Task<ActionResult> RemoveLike(string username)
		//{
		//	var sourceUserId = User.GetUserId();
		//	var sourceUser = await _likeRepository.GetUserWithLikes(sourceUserId);

		//	var removeLikeUserId = await _userRepository.GetUserByUsernameAsync(username);

		//	if(removeLikeUserId == null) return NotFound();

		//	if (sourceUser.UserName == username) return BadRequest("You caanot like yourself");


		//}

	}
}
