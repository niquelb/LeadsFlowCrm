using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;
using Google.Apis.PeopleService.v1;
using Google.Apis.Services;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
using LeadsFlowCrm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services;

/// <summary>
/// Base service for every other Google related service
/// </summary>
public class BaseGoogleServiceClass : IBaseGoogleServiceClass
{
	/// <summary> Base client service for creating all other services </summary>
	private BaseClientService.Initializer _service;

	/// <summary> Credentials object </summary>
	private UserCredential _credentials;

	public const string _appName = "LeadsFlow CRM";

	private readonly IApiHelper _apiHelper;
	private readonly LoggedInUser _loggedInUser;

	/// <summary> Scopes for the OAuth login </summary>
	private static readonly string[] _scopes = {
			GmailService.Scope.MailGoogleCom,
			PeopleServiceService.Scope.Contacts,
			Oauth2Service.Scope.UserinfoEmail,
			Oauth2Service.Scope.UserinfoProfile,
		};

	public BaseGoogleServiceClass(IApiHelper apiHelper, LoggedInUser loggedInUser)
	{
		_apiHelper = apiHelper;
		_loggedInUser = loggedInUser;
	}

    public CancellationToken CancellationToken { get; set; } = new CancellationToken();

    /// <summary>
    /// Method for getting a BaseClientService to instanciate other Google services
    /// </summary>
    /// <returns></returns>
    public async Task<BaseClientService.Initializer> GetServiceAsync()
	{
		_service ??= new BaseClientService.Initializer
			{
				HttpClientInitializer = await GetCredentialsAsync(),
				ApplicationName = _appName
			};

		return _service;
	}

	/// <summary>
	/// Method that generates the Credentials
	/// </summary>
	/// <returns>Generated Credentials object</returns>
	/// <exception cref="ArgumentNullException">If there's an error obtaining the client secrets</exception>
	private async Task<UserCredential> GenerateCredentialsAsync()
	{
		// We retrieve the client secrets from our API
		ClientSecrets? clientSecrets = await _apiHelper.GetGoogleClientSecretsAsync();

		if (clientSecrets == null)
		{
			throw new ArgumentNullException(nameof(clientSecrets));
		}

		var output = await GoogleWebAuthorizationBroker.AuthorizeAsync(
				clientSecrets,
				_scopes,
				"user",
				CancellationToken
			);

		return output;
	}

	/// <summary>
	/// Method for retrieving the Credentials object necessary for the consumption of the Google APIs
	/// </summary>
	/// <returns>Credentials for the Google APIs</returns>
	public async Task<UserCredential> GetCredentialsAsync()
	{
		// We generate the credentials if they don't yet exist (singleton pattern)
		if (_credentials == null)
		{
			/*
			 * We don't automatically do this in the constructor so that the Google sign in window doesn't appear until we request the
			 * credentials
			 */

			try
			{
				_credentials = await GenerateCredentialsAsync();
			}
			catch (Exception)
			{
				throw;
			}
		}

		return _credentials;
	}

	/// <summary>
	/// Method for re-authing a user if the token has expired or is invalid
	/// </summary>
	public async Task ReAuthorizeUserAsync()
	{
		//TODO: use a refresh token

		var credentials = await GetCredentialsAsync();

		await GoogleWebAuthorizationBroker.ReauthorizeAsync(
				credentials,
				CancellationToken
			);
	}

	/// <summary>
	/// Method for revoking the OAuth token from the logged in user
	/// </summary>
	/// <see cref="LoggedInUser"/>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException">If the LogggedInUser or it's token are null/empty</exception>
	/// <exception cref="Exception">If the request fails</exception>
	public async Task RevokeAuthAsync()
	{
        if (_loggedInUser == null || string.IsNullOrWhiteSpace(_loggedInUser.AssignedUser.OAuthToken))
        {
			throw new ArgumentNullException("Logged in user or OAuth token are null.");
        }

		/*
		 * We do the REST request manually since the in-built API requires a static context
		 */

		// API endpoint
		var tokenRevocationEndpoint = "https://accounts.google.com/o/oauth2/revoke";

		// We make the request passing in the token
		var request = new HttpRequestMessage(HttpMethod.Post, tokenRevocationEndpoint)
		{
			Content = new FormUrlEncodedContent(new[]
		{
			new KeyValuePair<string, string>("token", _loggedInUser.AssignedUser.OAuthToken)
		})
		};

		using HttpClient client = new();
		var response = await client.SendAsync(request);

		// Check if the token was revoked successfully
		if (response.IsSuccessStatusCode == false)
		{
			throw new Exception($"[{response.StatusCode}] → {await response.Content.ReadAsStringAsync()}");
		}
	}
}
