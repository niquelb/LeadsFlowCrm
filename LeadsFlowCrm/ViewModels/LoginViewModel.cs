using Caliburn.Micro;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Auth.OAuth2.Flows;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;
using Google.Apis.Services;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using LeadsFlowCrm.Services.ModelServices;
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
	private readonly IBaseGoogleServiceClass _baseService;
	private readonly IOAuthServiceClass _oAuthService;
	private readonly IUserService _userService;

	public LoginViewModel(IWindowManager windowManager,
					   ShellViewModel shellViewModel,
					   IBaseGoogleServiceClass baseService, 
					   IOAuthServiceClass oAuthService,
					   IUserService userService)
	{
		_windowManager = windowManager;
		_shellViewModel = shellViewModel;
		_baseService = baseService;
		_oAuthService = oAuthService;
		_userService = userService;
	}

	/*
	 * These properties are used to control the visibility of the loading spinner and the login form
	 */
	private bool _isLoading = false;
	private string _loadingText;

	public bool IsLoading
	{
		get { return _isLoading; }
		set
		{
			_isLoading = value;
			NotifyOfPropertyChange();
		}
	}

	public string LoadingText
	{
		get { return _loadingText; }
		set { 
			_loadingText = value;
			NotifyOfPropertyChange();
		}
	}


	/// <summary>
	/// Login method that handles the appropiate window redirections
	/// </summary>
	public async void Login()
	{
		IsLoading = true;
		LoadingText = "Please wait while we authenticate you...";

		bool isAuthenticated = await _userService.GoogleSignInAsync();

		if (isAuthenticated)
		{
			LoadingText = "Authentication successful...";

			// Open the shell view
			await _windowManager.ShowWindowAsync(_shellViewModel);

			// Close the login view
			await TryCloseAsync();
		}

		IsLoading = false;
		LoadingText = string.Empty;
	}
}
