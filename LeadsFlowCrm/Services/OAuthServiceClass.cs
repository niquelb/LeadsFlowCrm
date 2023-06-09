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
/// Class containing methods related to the user's authentication
/// </summary>
public class OAuthServiceClass : IOAuthServiceClass
{
	private readonly IBaseGoogleServiceClass _baseGoogleService;

	/// <summary> OAuth 2 service </summary>
	private Oauth2Service _oauth2Service;

	public OAuthServiceClass(IBaseGoogleServiceClass baseGoogleService)
	{
		_baseGoogleService = baseGoogleService;
	}

	/// <summary>
	/// Method for retrieving the OAuth 2.0 service object
	/// </summary>
	/// <returns>Oauth2Service object</returns>
	public async Task<Oauth2Service> GetOauthServiceAsync()
	{
		if (_oauth2Service == null)
		{
			try
			{
				_oauth2Service = new Oauth2Service(await _baseGoogleService.GetServiceAsync());
			}
			catch (Exception)
			{
				/*
				 * If the auth credentials are not valid
				 */

				await _baseGoogleService.ReAuthorizeUserAsync();

				_oauth2Service = new Oauth2Service(await _baseGoogleService.GetServiceAsync());
			}
		}

		return _oauth2Service;
	}

}
