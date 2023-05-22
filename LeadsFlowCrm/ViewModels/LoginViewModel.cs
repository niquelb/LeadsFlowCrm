using Caliburn.Micro;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
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

	public LoginViewModel(IWindowManager windowManager, ShellViewModel shellViewModel)
    {
		_windowManager = windowManager;
		_shellViewModel = shellViewModel;
	}

	public async void Login()
	{
		bool isAuthenticated = await GoogleSignIn();

		if (isAuthenticated)
		{
			// Close the login view
			await TryCloseAsync();

			// Open the shell view
			await _windowManager.ShowWindowAsync(_shellViewModel);
		}
	}

	/// <summary>
	/// Google Sign-In method
	/// </summary>
	/// <returns></returns>
	private static async Task<bool> GoogleSignIn()
	{
		// TODO: Replace logic with propietary API
		UserCredential credential;
		using (var stream = new FileStream("C:\\Users\\nique\\source\\repos\\LeadsFlowCrmApp\\LeadsFlowCrm\\client_secret.json", FileMode.Open, FileAccess.Read))
		{
			credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
				GoogleClientSecrets.FromStream(stream).Secrets,
				new[] { "email", "profile" },
				"user",
				CancellationToken.None
			);
		}

		var oauthService = new Oauth2Service(new BaseClientService.Initializer
		{
			HttpClientInitializer = credential,
			ApplicationName = "LeadsFlowCRM"
		});

		var userInfo = await oauthService.Userinfo.Get().ExecuteAsync();
		string email = userInfo.Email;
		string profileName = userInfo.Name;

		Trace.WriteLine(email, profileName);
		Trace.WriteLine(credential.Token.AccessToken, profileName);

		return credential != null;
	}
}
