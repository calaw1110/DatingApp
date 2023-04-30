using DatingApp.API.Data;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{

	[Authorize]
	public class UsersController : BaseApiController
	{
		private readonly DatingAppDataContext _context;

		public UsersController(DatingAppDataContext context)
		{
			this._context = context;
		}

		[AllowAnonymous]
		[HttpGet]
		public async Task<IEnumerable<AppUser>> GetUsers()
		{
			var users = await _context.Users.ToListAsync();

			return users;
		}

		[HttpGet("{id}")]
		public async Task<AppUser> GetUser(int id)
		{
			var user = await _context.Users.FindAsync(id);

			return user;
		}
	}
}