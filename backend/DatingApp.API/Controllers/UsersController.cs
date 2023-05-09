using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DatingApp.API.Controllers
{

	[Authorize]
	public class UsersController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;
		private readonly IPhotoService _photoService;

		public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
		{
			this._userRepository = userRepository;
			this._mapper = mapper;
			this._photoService = photoService;
		}


		[HttpGet]
		public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
		{
			var users = await _userRepository.GetMembersAsync();
			return Ok(users);
		}

		[HttpGet("{username}")]
		public async Task<ActionResult<MemberDto>> GetUserByName(string username)
		{
			var user = await _userRepository.GetMemberAsync(username);
			return Ok(user);
		}

		[HttpPut]
		public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
		{
			// 取得登入驗證資訊
			var username = User.GetUsername();

			// 取得登入者詳細資料
			var user = await _userRepository.GetUserByUsernameAsync(username);


			if (user == null) return NotFound();

			// 將dto資料更新至user資料
			_mapper.Map(memberUpdateDto, user);

			// 更新資料庫資料 
			if (await _userRepository.SavaAllAsync())
				// 更新成功 使用 status code 204 回傳 
				return NoContent();

			// 更新失敗 使用 status code 400 回傳
			return BadRequest("Failed to update user");
		}

		[HttpPost("add-photo")]
		public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
		{
			// 取得登入驗證資訊
			var username = User.GetUsername();

			// 取得登入者詳細資料
			var user = await _userRepository.GetUserByUsernameAsync(username);

			if (user == null) return NotFound();

			var result = await _photoService.AddPhotoAsync(file);

			if (result.Error != null) return BadRequest(result.Error.Message);

			var photo = new Photo
			{
				Url = result.SecureUrl.AbsoluteUri,
				PublicId = result.PublicId,
			};

			if (user.Photos.Count == 0) photo.IsMain = true;

			user.Photos.Add(photo);

			if (await _userRepository.SavaAllAsync())
			{
				// status 201 
				return CreatedAtAction(nameof(GetUsers), new { username = user.UserName }, _mapper.Map<PhotoDto>(photo));

				// status 200
				// return _mapper.Map<PhotoDto>(photo);

			}

			return BadRequest("Problem adding photo");
		}

		[HttpPut("set-main-photo/{photoId}")]
		public async Task<ActionResult> SetMainPhoto(int photoId)
		{
			var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

			if (user == null) return NotFound();

			var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

			if (photo == null) return NotFound();

			if (photo.IsMain) return BadRequest("this is already your main photo");

			var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

			if (currentMain != null) currentMain.IsMain = false;
			photo.IsMain = true;

			if (await _userRepository.SavaAllAsync()) return NoContent();

			return BadRequest("Problem setting the main photo");
		}

		[HttpDelete("delete-photo/{photoId}")]
		public async Task<ActionResult> DeletePhoto(int photoId)
		{
			var user = await _userRepository.GetUserByUsernameAsync(User.GetUsername());

			var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

			if (photo == null) return NotFound();

			if (photo.IsMain) return BadRequest("You cannot delete your main photo");

			if (photo.PublicId != null)
			{
				var result = await _photoService.DeletePhotoAsync(photo.PublicId);
				if (result.Error != null) return BadRequest(result.Error);
			}
			
			user.Photos.Remove(photo);

			if (await _userRepository.SavaAllAsync()) return Ok();

			return BadRequest("Problem delete the photo");
		}


	}
}