using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOs;
using DatingApp.API.Entities;
using DatingApp.API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace DatingApp.API.Controllers
{

	public class AccountController : BaseApiController
	{
		private readonly DatingAppDataContext _context;
		private readonly IUserRepository _userRepository;

		public ITokenService _tokenService { get; }
		public IMapper _mapper { get; }

		public AccountController(DatingAppDataContext context, ITokenService tokenService, IUserRepository userRepository, IMapper mapper)
		{
			this._context = context;
			this._tokenService = tokenService;
			this._userRepository = userRepository;
			this._mapper = mapper;
		}
		[HttpPost("register")] // POST: /api/account/register
		public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
		{
			if (await IsUserExists(registerDto.UserName)) return BadRequest("Username is taken");

			var user = _mapper.Map<AppUser>(registerDto);

			using var hmac = new HMACSHA512();


			user.UserName = registerDto.UserName.ToLower();
			user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
			user.PasswordSalt = hmac.Key;


			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			return new UserDto
			{
				Username = user.UserName,
				Token = _tokenService.CreateToken(user),
				KnownAs = user.KnownAs
			};
		}

		[HttpPost("login")]
		public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
		{
			var user = await _userRepository.GetUserByUsernameAsync(loginDto.Username);

			if (user == null) return Unauthorized("invalid username");

			using var hmac = new HMACSHA512(user.PasswordSalt);
			var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

			for (int i = 0; i < computedHash.Length; i++)
			{
				if (computedHash[i] != user.PasswordHash[i])
					return Unauthorized("invalid password");
			}


			return new UserDto
			{
				Username = user.UserName,
				Token = _tokenService.CreateToken(user),
				PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url,
				KnownAs = user.KnownAs
			};
		}



		private async Task<bool> IsUserExists(string username)
		{
			return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
		}
	}
}
