using LeadsFlowApiV2.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadsFlowApiV2.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthMethods _authFilter;

		public AuthController(IAuthMethods authFilter)
        {
			_authFilter = authFilter;
		}

        [HttpPost("Login")]
		public ActionResult Login(string OAuthToken, string UserName)
		{
			try
			{
				// TODO: check the OAuthToken validity

				string token = _authFilter.GetToken(OAuthToken, UserName);

				if (string.IsNullOrEmpty(token))
				{
					return ValidationProblem("There was an error generating the token");
				}

				return Ok(token);

			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}
	}
}
