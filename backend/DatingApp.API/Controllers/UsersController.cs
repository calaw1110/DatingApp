using AutoMapper;
using DatingApp.API.DTOs;
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

		public UsersController(IUserRepository userRepository, IMapper mapper)
		{
			this._userRepository = userRepository;
			this._mapper = mapper;
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
			var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			// 取得登入者資料
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
	}
}