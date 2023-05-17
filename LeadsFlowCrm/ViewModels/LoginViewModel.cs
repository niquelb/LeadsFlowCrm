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

	public LoginViewModel(IWindowManager windowManager)
    {
		_windowManager = windowManager;
	}

	public void Login()
	{
		bool isAuthenticated = true;

		if (isAuthenticated)
		{
			// Close the login view
			TryCloseAsync();

			// Open the shell view
			_windowManager.ShowWindowAsync(new ShellViewModel());
		}
	}
}
