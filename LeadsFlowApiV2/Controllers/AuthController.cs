using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using LeadsFlowApiV2.Auth;
using LeadsFlowApiV2.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;
using System.Text.Json;

namespace LeadsFlowApiV2.Controllers;

/// <summary>
/// Controller for all the methods related to authentication and authorization
/// </summary>
[Route("[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
	private readonly IAuthMethods _auth;
	private readonly IUserDAO _userDAO;

	public AuthController(IAuthMethods auth, IUserDAO userDAO)
	{
		_auth = auth;
		_userDAO = userDAO;
	}

	/// <summary>
	/// Login method for the API
	/// </summary>
	/// <param name="oAuthToken">Google OAuth token</param>
	/// <param name="email">User's email</param>
	/// <returns>
	/// [OK] if successful, 
	/// [BadRequest] if the OAuth is invalid, 
	/// [NotFound] if no user with that email exists,
	/// [Problem] if the token generation fails or there are any other problems
	/// </returns>
	[HttpPost("Login")]
	public async Task<ActionResult> Login([FromBody] UserEmailOauth user)
	{
		try
		{
			/*
			 * We check the validity of the OAuth token
			 */
			if (await _auth.CheckOauthToken(user.OAuthToken) == false)
			{
				return BadRequest("OAuth Token is not valid");
			}

			// We retrieve the user ID
			string? userId = await _userDAO.GetUserByEmail(user.Email);

			/*
			 * We check if the user exists in the BD
			 */
			if (string.IsNullOrWhiteSpace(userId))
			{
				// TODO: register the user if it doesn't exist
				return NotFound("No user was found with the given email");
			}

			// We update the given user's record on the DB with the new OAuth token
			await _auth.UpdateUser(userId, user.OAuthToken);

			// We generate the token
			string token = _auth.GetToken(userId);

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

	/// <summary>
	/// Method for replacing the OAuth token stored in the DB for a given user with a new one
	/// </summary>
	/// <param name="UserId">ID of the user</param>
	/// <param name="OAuthToken">New OAuth token</param>
	/// <returns>
	/// [Ok] if successful,
	/// [BadRequest] if the OAuth token is invalid,
	/// [NotFound] if no user with the given ID is found,
	/// [Problem] if there is any other issue
	/// </returns>
	[HttpPut("RefreshOAuth")]
	public async Task<ActionResult> RefreshOAuthToken(string UserId, [FromBody] string OAuthToken)
	{
		try
		{
			/*
			 * We check the validity of the OAuth token
			 */
			if (await _auth.CheckOauthToken(OAuthToken) == false)
			{
				return BadRequest("OAuth Token is not valid");
			}

			/*
			 * We check if the ID is valid
			 */
			User? checkUser = await _userDAO.GetUser(UserId);

			if (checkUser == null)
			{
				return NotFound($"No user was found with the given ID: { UserId }");
			}

			// We update the user
			await _auth.UpdateUser(UserId, OAuthToken);

			return Ok(UserId);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	/// <summary>
	/// Method for getting the necessary client secrets for the Google sign in using a private API key
	/// </summary>
	/// <param name="ApiKey">API key needed to access this method</param>
	/// <returns>
	/// [Ok] if successful,
	/// [Unauthorized] if the API key is invalid,
	/// [NotFound] if the client_secrets.json file is not found in the server
	/// [Problem] if there is an issue parsing the json file or similar problems
	/// </returns>
	[HttpGet("ClientSecrets")]
	public ActionResult Get(string ApiKey)
	{
		/*
		 * We first check if the provided API key is valid for the request
		 */
		if (_auth.CheckApiKey(ApiKey) == false)
		{
			return Unauthorized("Invalid API Key");
		}

		try
		{
			/*
			 * We read and parse the contents of the JSON file that stores the client secrets for the Google API
			 */
			var assembly = Assembly.GetExecutingAssembly();
			var resourceName = "LeadsFlowApiV2.Resources.client_secrets.json";

			using Stream? stream = assembly.GetManifestResourceStream(resourceName);

			if (stream == null)
			{
				return NotFound("Configuration JSON not found.");
			}

			using StreamReader reader = new(stream);

			string jsonContents = reader.ReadToEnd();

			JsonDocument jsonDocument = JsonDocument.Parse(jsonContents);

			JsonElement root = jsonDocument.RootElement;
			JsonElement installedElement = root.GetProperty("installed");

			ClientSecrets? clientSecrets = installedElement.Deserialize<ClientSecrets>();

			if (clientSecrets == null)
			{
				return Problem("Problem parsing the configuration JSON");
			}

			return Ok(clientSecrets);
		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}
}
