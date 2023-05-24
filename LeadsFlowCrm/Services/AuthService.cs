using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;
using LeadsFlowCrm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services;

/// <summary>
/// Class containing methods related to the user's authentication
/// </summary>
public class AuthService : IAuthService
{
	/// <summary> Credentials object </summary>
	private UserCredential _credentials;

	/// <summary> Scopes for the OAuth login </summary>
	private static readonly string[] _scopes = {
			GmailService.Scope.MailGoogleCom,
			Oauth2Service.Scope.UserinfoEmail,
			Oauth2Service.Scope.UserinfoProfile,
		};

	private readonly IApiHelper _apiHelper;

	public AuthService(IApiHelper apiHelper)
	{
		_apiHelper = apiHelper;
	}

	/// <summary>
	/// Method that generates the Credentials
	/// </summary>
	/// <returns>Generated Credentials object</returns>
	/// <exception cref="ArgumentNullException">If there's an error obtaining the client secrets</exception>
	private async Task<UserCredential> GenerateCredentials()
	{
		// We retrieve the client secrets from our API
		ClientSecrets? clientSecrets = await _apiHelper.GetGoogleClientSecrets();

		if (clientSecrets == null)
		{
			throw new ArgumentNullException(nameof(clientSecrets));
		}

		return await GoogleWebAuthorizationBroker.AuthorizeAsync(
				clientSecrets,
				_scopes,
				"user",
				CancellationToken.None
			);
	}

	/// <summary>
	/// Method for retrieving the Credentials object necessary for the consumption of the Google APIs
	/// </summary>
	/// <returns>Credentials for the Google APIs</returns>
	public async Task<UserCredential> GetCredentials()
	{
		// We genterate the credentials if they don't yet exist (singleton pattern)
		if (_credentials == null)
		{
			/*
			 * We don't automatically do this in the constructor so that the Google sign in window doesn't appear until we request the
			 * credentials
			 */

			try
			{
				_credentials = await GenerateCredentials();
			}
			catch (Exception)
			{
				throw;
			}
		}

		return _credentials;
	}
}
