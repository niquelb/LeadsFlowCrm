using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

	public void Login()
	{
		bool isAuthenticated = true;

		if (isAuthenticated)
		{
			// Close the login view
			TryCloseAsync();

			// Open the shell view
			_windowManager.ShowWindowAsync(_shellViewModel);
		}
	}
}
