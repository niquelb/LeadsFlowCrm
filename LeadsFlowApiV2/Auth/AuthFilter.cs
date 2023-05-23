using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LeadsFlowApiV2.Auth;

public class AuthFilter : IAuthFilter
{
	private readonly IConfiguration _configuration;

	public AuthFilter(IConfiguration configuration)
	{
		_configuration = configuration;
	}

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
}
