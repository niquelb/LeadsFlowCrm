using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LeadsFlowApiV2.Auth;

/// <summary>
/// Class that stores all the authentication related methods
/// </summary>
public class AuthMethods : IAuthMethods
{
	private readonly IConfiguration _configuration;

	public AuthMethods(IConfiguration configuration)
	{
		_configuration = configuration;
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
			return false;
		}

		var storedApiKey = _configuration.GetValue<string>(AuthConstants.ApiKeyRoute);

		if (storedApiKey.Equals(apiKey) == false)
		{
			return false;
		}

		return true;
	}

	/// <summary>
	/// Method for creating and returning a JWT token
	/// </summary>
	/// <param name="oauthToken">OAuth token used for the external APIs</param>
	/// <param name="username">User name</param>
	/// <returns>The token</returns>
	public string GetToken(string oauthToken, string username)
	{
		List<Claim> claims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, username),
			new Claim(ClaimTypes.Role, "base"),
			new Claim("oauth_token", oauthToken)
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
