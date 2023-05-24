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
	private const string tokenCheckUrl = "https://www.googleapis.com/oauth2/v1/tokeninfo?access_token=";
	private HttpClient client = new();

	public AuthMethods(IConfiguration configuration)
	{
		_configuration = configuration;

		InitializeClient();
	}

	private void InitializeClient()
	{
		client = new HttpClient();
		client.BaseAddress = new Uri(tokenCheckUrl);
		client.DefaultRequestHeaders.Accept.Clear();
		client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));
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

		string url = $"{tokenCheckUrl}{token}";

		using (HttpResponseMessage resp = await client.GetAsync(url))
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
