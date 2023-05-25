using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using LeadsFlowCrm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

	/// <summary> Scopes for the OAuth login </summary>
	private static readonly string[] _scopes = {
			GmailService.Scope.MailGoogleCom,
			GmailService.Scope.GmailReadonly,
			Oauth2Service.Scope.UserinfoEmail,
			Oauth2Service.Scope.UserinfoProfile,
		};

	public BaseGoogleServiceClass(IApiHelper apiHelper)
	{
		_apiHelper = apiHelper;
	}

	/// <summary>
	/// Method for getting a BaseClientService to instanciate other Google services
	/// </summary>
	/// <returns></returns>
	public async Task<BaseClientService.Initializer> GetServiceAsync()
	{
		if (_service == null)
		{
			_service = new BaseClientService.Initializer
			{
				HttpClientInitializer = await GetCredentialsAsync(),
				ApplicationName = _appName
			};
		}

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

		// TODO: make cancellation token accessible in case a reauth is needed.
		var cancelToken = new CancellationToken();

		if (clientSecrets == null)
		{
			throw new ArgumentNullException(nameof(clientSecrets));
		}

		var output = await GoogleWebAuthorizationBroker.AuthorizeAsync(
				clientSecrets,
				_scopes,
				"user",
				cancelToken
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
}
