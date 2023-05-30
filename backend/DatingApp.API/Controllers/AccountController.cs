using AutoMapper;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Extensions;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
	public class AccountController : BaseApiController
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly IConfiguration _config;

		public ITokenService _tokenService { get; }
		public IMapper _mapper { get; }

		public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper,IConfiguration config)
		{
			this._userManager = userManager;
			this._tokenService = tokenService;
			this._mapper = mapper;
			this._config = config;
		}

		[HttpPost("register")] // POST: /api/account/register
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			if (await IsUserExists(registerDto.UserName)) return BadRequest("Username is taken");

			var user = _mapper.Map<AppUser>(registerDto);

			user.UserName = registerDto.UserName.ToLower();

			var result = await _userManager.CreateAsync(user, registerDto.Password);

			if (!result.Succeeded) return BadRequest(result.Errors.ToString());

			var roleResult = await _userManager.AddToRoleAsync(user, "Member");

			if (!roleResult.Succeeded) return BadRequest(result.Errors);

			return new UserDto
			{
				Username = user.UserName,
				Token = await _tokenService.CreateToken(user),
				PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
				KnownAs = user.KnownAs,
				Gender = user.Gender,
			};
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _userManager.Users
				.Include(p => p.Photos)
				.SingleOrDefaultAsync(x => x.UserName == loginDto.Username.ToAESDecrypt());

			if (user == null) return Unauthorized("Invalid account or password");

			var result = await _userManager.CheckPasswordAsync(user, loginDto.Password.ToAESDecrypt());

			if (!result) return Unauthorized("Invalid account or password");

			return new UserDto
			{
				Username = user.UserName,
				Token = await _tokenService.CreateToken(user),
				PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
				KnownAs = user.KnownAs,
				Gender = user.Gender,
			};
		}

		private async Task<bool> IsUserExists(string username)
		{
			return await _userManager.Users.AnyAsync(x => x.UserName == username.ToLower());
		}
	}
}