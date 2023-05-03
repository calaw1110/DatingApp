using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{

	[Authorize]
	public class UsersController : BaseApiController
	{
		private readonly IUserRepository _userRepository;
		private readonly IMapper _mapper;

		public UsersController(IUserRepository userRepository,IMapper mapper)
		{
			this._userRepository = userRepository;
			this._mapper = mapper;
		}


		[HttpGet]
		public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
		{
			//var users = await _userRepository.GetUsersAsync();

			//var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
			var usersToReturn = await _userRepository.GetMembersAsync();
			return Ok(usersToReturn);
		}

		[HttpGet("{username}")]
		public async Task<ActionResult<MemberDto>> GetUserByName(string username)
		{
			var user = await _userRepository.GetUserByUsernameAsync(username);

			var usersToReturn = _mapper.Map<MemberDto>(user);

			return Ok(usersToReturn);
		}
	}
}