using DataAccess.DataAccess.DAO;
using LeadsFlowApiV2.Auth;
using LeadsFlowApiV2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LeadsFlowApiV2.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthMethods _auth;
		private readonly IUserDAO _userDAO;

		public AuthController(IAuthMethods authFilter, IUserDAO userDAO)
		{
			_auth = authFilter;
			_userDAO = userDAO;
		}

        [HttpPost("Login")]
		public async Task<ActionResult> Login(string OAuthToken, string Email)
		{
			try
			{
				if (await _auth.CheckOauthToken(OAuthToken) == false)
				{
					return BadRequest("OAuth Token is not valid");
				}

				string? userId = await _userDAO.GetUserByEmail(Email);

				if (string.IsNullOrWhiteSpace(userId))
				{
					// TODO: register the user if it doesn't exist
					return NotFound("No user was found with the given email");
				}

				string token = _auth.GetToken(OAuthToken, Email, userId);

				if (string.IsNullOrEmpty(token))
				{
					return Problem("There was an error generating the token");
				}

				return Ok(new LoggedInUser { Id = userId, Token = token });

			}
			catch (Exception ex)
			{
				return Problem(ex.Message);
			}
		}
	}
}
