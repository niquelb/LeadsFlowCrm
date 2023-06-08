using LeadsFlowCrm.Models;
using LeadsFlowCrm.Utils;
using System;
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
	private readonly LoggedInUser _user;

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
	/// <param name="OAuthToken">Google OAuth token from the Google sign in</param>
	/// <param name="Email">User's email</param>
	/// <returns></returns>
	/// <exception cref="UnauthorizedAccessException">If login call to API fails, most likely due to an invalid OAuth token</exception>
	/// <see cref="LoggedInUser"/>
	public async Task AuthenticateAsync(string OAuthToken, string Email)
	{
		// We create the body
		var body = new
		{
			Email,
			OAuthToken,
		};

		// We encode the body
		var encodedBody = new StringContent(JsonSerializer.Serialize<object>(body), Encoding.UTF8, "application/json");

		// We make the request
		using HttpResponseMessage resp = await _apiClient.PostAsync("Auth/Login", encodedBody);
		if (resp.IsSuccessStatusCode == false)
		{
			throw new UnauthorizedAccessException(resp.ReasonPhrase);
		}

		LoggedInUser output = await resp.Content.ReadAsAsync<LoggedInUser>();

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
		var userInfo = await oauthService.Userinfo.Get().ExecuteAsync();

		// We collect the user's info
		string email = userInfo.Email;
		string token = await credentials.GetAccessTokenForRequestAsync();

		Trace.WriteLine(token, nameof(token));

		// We authenticate in our API
		await AuthenticateAsync(token, email);

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
}
