using DataAccess.DataAccess.DAO;
using DataAccess.Models;
using LeadsFlowApiV2.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

namespace LeadsFlowApiV2.Auth;

/// <summary>
/// Class that stores all the authentication related methods
/// </summary>
public class AuthMethods : IAuthMethods
{
	private readonly IConfiguration _configuration;
	private readonly IUserDAO _user;
	private const string _tokenCheckUrl = "https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=";
	private HttpClient _client = new();

	public AuthMethods(IConfiguration configuration, IUserDAO user)
	{
		_configuration = configuration;
		_user = user;
		InitializeClient();
	}

	/// <summary>
	/// Method for initializing the HttpClient
	/// </summary>
	private void InitializeClient()
	{
		_client = new HttpClient();
		_client.BaseAddress = new Uri(_tokenCheckUrl);
		_client.DefaultRequestHeaders.Accept.Clear();
		_client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
	}

	/// <summary>
	/// Method for updating the OAuth token for a user in the DB
	/// </summary>
	/// <param name="id">ID of the user</param>
	/// <param name="oAuthToken">New OAuth token</param>
	/// <returns></returns>
	public async Task UpdateUser(string id, string oAuthToken)
	{
		User user = new()
		{
			Id = id,
			OAuthToken = oAuthToken
		};

		await _user.UpdateUser(user);
	}

	/// <summary>
	/// Method for checking if the submitted API key is valid
	/// </summary>
	/// <param name="apiKey">API key to be checked</param>
	/// <returns>TRUE -> Key is valid, FALSE -> Key is NOT valid.</returns>
	public bool CheckApiKey(string apiKey)
	{
		if (string.IsNullOrWhiteSpace(apiKey))
		{
			throw new ArgumentNullException(nameof(apiKey));
		}

		var storedApiKey = _configuration.GetValue<string>(AuthConstants.ApiKeyRoute);

		if (storedApiKey.Equals(apiKey) == false)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// Method for checking if the supplied OAuth 2.0 token from Google is valid
	/// </summary>
	/// <param name="token">Token that will be checked</param>
	/// <returns>TRUE -> Token is valid, FALSE -> Token is NOT valid.</returns>
	public async Task<bool> CheckOauthToken(string token)
	{
		if (string.IsNullOrWhiteSpace(token))
		{
			throw new ArgumentNullException(nameof(token));
		}

		string url = $"{_tokenCheckUrl}{token}";

		using (HttpResponseMessage resp = await _client.GetAsync(url))
		{
			return resp.IsSuccessStatusCode;
		}
	}

	/// <summary>
	/// Method for creating and returning a JWT token
	/// </summary>
	/// <param name="oauthToken">OAuth token used for the external APIs</param>
	/// <param name="email">User name</param>
	/// <returns>The token</returns>
	public string GetToken(string id)
	{
		List<Claim> claims = new List<Claim>
		{
			new Claim(ClaimTypes.Role, "base"),
			new Claim("id", id)
		};

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Authentication:Token").Value));

		var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

		var token = new JwtSecurityToken(
			claims: claims, 
			expires: DateTime.Now.AddDays(2), 
			signingCredentials: credentials
			);

		var jwtToken = new JwtSecurityTokenHandler().WriteToken(token);

		return jwtToken;
	}
}
