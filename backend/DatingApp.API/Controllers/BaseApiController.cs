using DatingApp.API.Helper;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
	[ServiceFilter(typeof(LogUserActivityHelper))]
	[ApiController]
	[Route("api/[controller]")]
	public class BaseApiController : ControllerBase
	{
		public BaseApiController()
		{

		}
	}
}
