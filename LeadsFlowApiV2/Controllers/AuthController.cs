using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadsFlowApiV2.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		[HttpPost("Login")]
		public ActionResult Login(string OAuthToken)
		{
			return Ok(OAuthToken);
		}
	}
}
