using DatingApp.API.Data;
using DatingApp.API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{

	public class BuggyController : BaseApiController
	{
		private readonly DatingAppDataContext _context;

		public BuggyController(DatingAppDataContext context)
		{
			this._context = context;
		}
		[Authorize]
		[HttpGet("auth")]
		public ActionResult<string> GetSecret()
		{
			return Ok();
		}
		[HttpGet("not-found")]
		public ActionResult<AppUser> GetNotFound()
		{
			var thing = _context.Users.Find(-1);

			if (thing == null) return NotFound();

			return thing;
		}
		[HttpGet("server-error")]
		public ActionResult<string> GetServerError()
		{

			var thing = _context.Users.Find(-1);

			var thingToReturn = thing.ToString();

			return thingToReturn;
		}

		[HttpGet("bad-request")]
		public ActionResult<AppUser> GetBadRequest()
		{
			return BadRequest("This was not a good request");
		}

	}
}
