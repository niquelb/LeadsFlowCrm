using Google.Apis.Oauth2.v2.Data;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services.ModelServices;

/// <summary>
/// Services for the User model 
/// </summary>
/// <see cref="User"/>
public class UserService : IUserService
{
	private readonly HttpClient _apiClient;
	private readonly IBaseGoogleServiceClass _baseGoogleService;
	private readonly IOAuthServiceClass _oAuthService;
	private LoggedInUser _user;

	public UserService(
		IApiHelper apiHelper,
		IBaseGoogleServiceClass baseGoogleService,
		IOAuthServiceClass oAuthService,
		LoggedInUser user)
	{
		_apiClient = apiHelper.ApiClient;
		_baseGoogleService = baseGoogleService;
		_oAuthService = oAuthService;
		_user = user;
	}

	/// <summary>
	/// Method for logging into the API and retrieving and storing the token in the LoggedInUser model
	/// </summary>
	/// <param name="oAuthToken">Google OAuth token from the Google sign in</param>
	/// <param name="email">User's email</param>
	/// <returns></returns>
	/// <exception cref="UnauthorizedAccessException">If login call to API fails, most likely due to an invalid OAuth token</exception>
	/// <exception cref="Exception">If the request fails for any other reason</exception>
	/// <see cref="LoggedInUser"/>
	public async Task AuthenticateAsync(string oAuthToken, string email, string name)
	{
		// We create the body
		var body = new
		{
			email,
			oAuthToken,
		};

		// We encode the body
		var encodedBody = new StringContent(JsonSerializer.Serialize<object>(body), Encoding.UTF8, "application/json");

		LoggedInUser? output = null;

		// We make the request
		using HttpResponseMessage resp = await _apiClient.PostAsync("Auth/Login", encodedBody);
		if (resp.IsSuccessStatusCode == false)
		{
			if (resp.StatusCode == HttpStatusCode.BadRequest)
			{
				throw new UnauthorizedAccessException(resp.ReasonPhrase);
			}
			if (resp.StatusCode == HttpStatusCode.NotFound)
			{
				// If the user doesn't exist we sign in the user, this returns a LoggedInUser like the login
				output = await ApiSignInAsync(email: email, oAuthToken: oAuthToken, name: name);
			}
			else
			{
				throw new Exception(resp.ReasonPhrase);
			}
		}

		/*
		 * If the login is unsuccessful and we have to do a sign in we will already have this variable
		 * assigned since the SignIn method of the API also returns a LoggedInUser object, if not we'll
		 * have to parse the login response
		 */
        output ??= await resp.Content.ReadAsAsync<LoggedInUser>();

		_user.Token = output.Token;
		_user.Id = output.Id;
		_user.AssignedUser = await GetUserAsync(output.Id, output.Token);
	}

	/// <summary>
	/// Google Sign-In method
	/// </summary>
	/// <returns></returns>
	public async Task<bool> GoogleSignInAsync()
	{
		// We retrieve the credentials
		var credentials = await _baseGoogleService.GetCredentialsAsync();

		if (credentials == null)
		{
			return false;
		}

		// We retrieve the OAuth service
		var oauthService = await _oAuthService.GetOauthServiceAsync();

		// We retrieve the user's profile information
		Userinfo userInfo;

		/*
		 * We do this in a Try-Catch because if the scopes for the OAuth request changes this throws an exception,
		 * shouldn't happen often
		 */
		try
		{
			userInfo = await oauthService.Userinfo.Get().ExecuteAsync();
		}
		catch (Exception)
		{
			await _baseGoogleService.ReAuthorizeUserAsync();
			userInfo = await oauthService.Userinfo.Get().ExecuteAsync();
		}

		// We collect the user's info
		string email = userInfo.Email;
		string name = userInfo.Name;
		string token = await credentials.GetAccessTokenForRequestAsync();

		Trace.WriteLine(token, nameof(token));

		// We authenticate in our API
		await AuthenticateAsync(token, email, name);

		return true;
	}

	/// <summary>
	/// Method for generating a User model with the ID provided
	/// </summary>
	/// <param name="id">User ID</param>
	/// <param name="token">Auth token</param>
	/// <returns>User model</returns>
	/// <exception cref="UnauthorizedAccessException">If the token is invalid</exception>
	/// <exception cref="ArgumentException">If there isn't a user with the given ID</exception>
	/// <exception cref="Exception">Any other error</exception>
	public async Task<User> GetUserAsync(string id, string token)
	{
		_apiClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

		using HttpResponseMessage resp = await _apiClient.GetAsync($"api/Users/{id}");
		if (resp.IsSuccessStatusCode == false)
		{
			if (resp.StatusCode == HttpStatusCode.Unauthorized)
			{
				throw new UnauthorizedAccessException(resp.ReasonPhrase);
			}

			if (resp.StatusCode == HttpStatusCode.NotFound)
			{
				throw new ArgumentException(resp.ReasonPhrase);
			}

			throw new Exception(resp.ReasonPhrase);
		}

		var output = await resp.Content.ReadAsAsync<User>();

		//TODO: output.Organization = getOrg(output.OrganizationId)

		return output;
	}

	/// <summary>
	/// Method for logging out the user, removing all stored info and revoking the OAuth token
	/// </summary>
	/// <returns></returns>
	public async Task LogoutUserAsync()
	{
		try
		{
			// We revoke the OAuth token
			await _baseGoogleService.RevokeAuthAsync();

			// We clear the user info
			_user = new();

			//TODO: Revoke token from proprietary API
		}
		catch (Exception)
		{
			throw;
		}
	}

	/// <summary>
	/// Method for signing in the user if it doesn't exist and getting it's token
	/// </summary>
	/// <param name="email">Email address</param>
	/// <param name="oAuthToken">Oauth token</param>
	/// <exception cref="Exception">If there is a problem signin in the user</exception>
	/// <returns>Newly signed in user's token</returns>
	private async Task<LoggedInUser> ApiSignInAsync(string email, string oAuthToken, string name)
	{
		// We create the body
		// We create the body of the request
		var bodyMap = new Dictionary<string, string?>
		{
			["email"] = email,
			["oAuthToken"] = oAuthToken,
			["displayName"] = name,
		};

		// We parse the body to a Json string
		string bodyStr = JsonSerializer.Serialize(bodyMap);

		// We create the content object for the request
		StringContent body = new(bodyStr, Encoding.UTF8, "application/json");

		// We make the request
		using HttpResponseMessage resp = await _apiClient.PostAsync("Auth/SignIn", body);
		if (resp.IsSuccessStatusCode == false)
		{
			throw new Exception(resp.ReasonPhrase);
		}

		LoggedInUser output = await resp.Content.ReadAsAsync<LoggedInUser>();

		return output;
	}
}
