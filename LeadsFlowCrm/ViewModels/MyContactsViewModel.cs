using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class MyContactsViewModel : Screen
{
    public MyContactsViewModel()
    {
    }

	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		ShowLoadingScreen = true;

		await Task.Delay(1000, cancellationToken);

		ShowLoadingScreen = false;
		ShowEmptyScreen = true;

		await Task.Delay(1000, cancellationToken);

		ShowEmptyScreen = false;
		ShowContent = true;
	}

	public string DisplayHeader { get; set; } = "My Contacts";

    /*
	 * Private backing fields
	 */
    private bool _showContent;
	private bool _showEmptyScreen;
	private bool _showLoadingScreen;

	/// <summary>
	/// Controls wether the main content is showing or not
	/// </summary>
	public bool ShowContent
	{
		get { return _showContent; }
		set { 
			_showContent = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls wether the loading screen is showing or not
	/// </summary>
	public bool ShowLoadingScreen
	{
		get { return _showLoadingScreen; }
		set { 
			_showLoadingScreen = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls wether the "empty" screen is showing or not
	/// </summary>
	public bool ShowEmptyScreen
	{
		get { return _showEmptyScreen; }
		set {
			_showEmptyScreen = value;
			NotifyOfPropertyChange();
		}
	}
}
