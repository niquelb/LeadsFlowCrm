using LeadsFlowApiV2.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadsFlowApiV2.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthMethods _auth;

		public AuthController(IAuthMethods authFilter)
        {
			_auth = authFilter;
		}

        [HttpPost("Login")]
		public async Task<ActionResult> Login(string OAuthToken, string UserName)
		{
			try
			{
				if (await _auth.CheckOauthToken(OAuthToken) == false)
				{
					return BadRequest("OAuth Token is not valid");
				}

				string token = _auth.GetToken(OAuthToken, UserName);

				if (string.IsNullOrEmpty(token))
				{
					return Problem("There was an error generating the token");
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
