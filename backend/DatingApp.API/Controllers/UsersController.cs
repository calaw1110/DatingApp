using DatingApp.API.Data;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]  //  /api/users
	public class UsersController : ControllerBase
	{
		private readonly DatingAppDataContext _context;

		public UsersController(DatingAppDataContext context)
		{
			this._context = context;
		}

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