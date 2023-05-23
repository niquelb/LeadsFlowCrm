using Caliburn.Micro;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class LoginViewModel : Screen
{
	private readonly IWindowManager _windowManager;
	private readonly ShellViewModel _shellViewModel;
	private readonly IApiHelper _apiHelper;

	public LoginViewModel(IWindowManager windowManager, ShellViewModel shellViewModel, IApiHelper apiHelper)
    {
		_windowManager = windowManager;
		_shellViewModel = shellViewModel;
		_apiHelper = apiHelper;
	}

	/// <summary>
	/// Login method that handles the appropiate window redirections
	/// </summary>
	public async void Login()
	{
		bool isAuthenticated = await GoogleSignIn();

		if (isAuthenticated)
		{
			// Open the shell view
			await _windowManager.ShowWindowAsync(_shellViewModel);

			// Close the login view
			await TryCloseAsync();
		}
	}

	/// <summary>
	/// Google Sign-In method
	/// </summary>
	/// <returns></returns>
	private async Task<bool> GoogleSignIn()
	{
		ClientSecrets? clientSecrets = await _apiHelper.GetGoogleClientSecrets();

		if (clientSecrets == null)
		{
			throw new ArgumentNullException(nameof(clientSecrets));
		}

		UserCredential credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
				clientSecrets,
				new[] { "email", "profile" },
				"user",
				CancellationToken.None
			);

		var oauthService = new Oauth2Service(new BaseClientService.Initializer
		{
			HttpClientInitializer = credential,
			ApplicationName = "LeadsFlowCRM"
		});

		var userInfo = await oauthService.Userinfo.Get().ExecuteAsync();
		string email = userInfo.Email;
		string profileName = userInfo.Name;

		return credential != null;
	}
}
