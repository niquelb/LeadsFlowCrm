﻿using DataAccess.DataAccess.DAO;
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
	/// [Problem] if the token generation fails or there are any other problems with the API
	/// </returns>
	[HttpPost("Login")]
	public async Task<ActionResult> Login([FromBody] LoginUser loggedInUser)
	{
		try
		{
			/*
			 * We check the validity of the OAuth token
			 */
			if (await _auth.CheckOauthToken(loggedInUser.OAuthToken) == false)
			{
				return BadRequest("OAuth Token is not valid");
			}

			// We retrieve the user from the BD using the email
			var user = (await _userDAO.GetUsers($"email like '{loggedInUser.Email}'")).FirstOrDefault();

			/*
			 * We check if the user exists in the BD
			 */
			if (user == null || string.IsNullOrWhiteSpace(user.Id))
			{
				return NotFound("No user was found with the given email");
			}

			// We update the given user's record on the DB with the new OAuth token
			await _auth.UpdateUser(user.Id, loggedInUser.OAuthToken);

			// We generate the token
			string token = _auth.GetToken(user.Id);

			if (string.IsNullOrEmpty(token))
			{
				return Problem("There was an error generating the token");
			}

			return Ok(new LoggedInUser { Id = user.Id, Token = token });

		}
		catch (Exception ex)
		{
			return Problem(ex.Message);
		}
	}

	/// <summary>
	/// Sign in method for the API
	/// </summary>
	/// <param name="user">User to be signed in</param>
	/// <returns>
	/// [OK] if successful, token is returned, 
	/// [BadRequest] if the user already exists or if the email is null/blank, 
	/// [Problem] if the token generation fails or there are any other problems with the API
	/// </returns>
	[HttpPost("SignIn")]
	public async Task<ActionResult> SignIn([FromBody] User user)
	{
		try
		{
            if (string.IsNullOrWhiteSpace(user.Email))
            {
				return BadRequest("Email cannot be null");
            }

			var apiUser = (await _userDAO.GetUsers($"email like '{user.Email}'")).FirstOrDefault();

			// We check if the user already exists
			if (apiUser != null)
			{
				return BadRequest("User already exists");
			}

			// We create the user
			await _userDAO.InsertUser(user);

			// We do the query again to obtain the ID, this time it should work
			var insertedUser = (await _userDAO.GetUsers($"email like '{user.Email}'")).FirstOrDefault();

			if (insertedUser == null || string.IsNullOrWhiteSpace(insertedUser.Id))
			{
				return Problem("User was not created correctly");
			}

			// We generate the token
			string token = _auth.GetToken(insertedUser.Id);

			if (string.IsNullOrEmpty(token))
			{
				return Problem("There was an error generating the token");
			}

			return Ok(new LoggedInUser { Id = insertedUser.Id, Token = token });
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
