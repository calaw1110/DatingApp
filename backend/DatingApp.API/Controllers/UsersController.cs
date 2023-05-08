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
			// ���o�n�J���Ҹ�T
			var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

			// ���o�n�J�̸��
			var user = await _userRepository.GetUserByUsernameAsync(username);


			if (user == null) return NotFound();

			// �Ndto��Ƨ�s��user���
			_mapper.Map(memberUpdateDto, user);

			// ��s��Ʈw��� 
			if (await _userRepository.SavaAllAsync())
				// ��s���\ �ϥ� status code 204 �^�� 
				return NoContent();

			// ��s���� �ϥ� status code 400 �^��
			return BadRequest("Failed to update user");
		}
	}
}