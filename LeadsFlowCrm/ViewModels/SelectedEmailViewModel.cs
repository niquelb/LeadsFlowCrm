using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LeadsFlowCrm.ViewModels;

public class SelectedEmailViewModel : Screen
{
	private readonly IGmailServiceClass _gmailServiceClass;
	private readonly IEventAggregator _event;

	public SelectedEmailViewModel(IGmailServiceClass gmailServiceClass,
							   IEventAggregator @event)
    {
		_gmailServiceClass = gmailServiceClass;
		_event = @event;
	}

    protected async override void OnViewLoaded(object view)
	{
		base.OnViewLoaded(view);

		LoadingText = "Fetching requested email...";
		IsLoading = true;

		// We retrieve the selected email
		Email email = _gmailServiceClass.SelectedEmail ?? new();

		// We parse and decode the body, this also populates the "EncodedBody" field
		email.Body = await Task.FromResult(_gmailServiceClass.GetProcessedBody(email));

		await Task.Delay(500);

		SelectedEmail = email;

		// We render the body
		Body = email.Body;

		IsLoading = false;
	}

	protected async override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
	{
		await base.OnDeactivateAsync(close, cancellationToken);

		// This is just to avoid a weird UI glitch
		LoadingText = "Loading inbox...";
		IsLoading = true;
		await Task.Delay(50);

		_gmailServiceClass.SelectedEmail = null;
	}

	/// <summary>
	/// Method that will redirect to the inbox
	/// </summary>
	public async void Back()
	{
		await _event.PublishOnUIThreadAsync(new NavigationEvent(NavigationEvent.NavigationRoutes.Inbox));
	}

	/*
	 * Selected email
	 */
	private Email _selectedEmail = new();

	public Email SelectedEmail
	{
		get { return _selectedEmail; }
		set { 
			_selectedEmail = value;
			NotifyOfPropertyChange();
		}
	}

	/*
	 * Email body to be rendered by the WebBrowser control
	 */
	private string _body = "Loading email body...";

	public string Body
	{
		get { return _body; }
		set { 
			_body = value;
			NotifyOfPropertyChange();
		}
	}

	/*
	 * This property represents the text displayed during the loading screen
	 */
	private string _loadingText;

	public string LoadingText
	{
		get { return _loadingText; }
		set { 
			_loadingText = value;
			NotifyOfPropertyChange();
		}
	}

	/*
	 * This property is used to control the visibility of the loading spinner
	 */
	private bool _isLoading = true;

	public bool IsLoading
	{
		get { return _isLoading; }
		set { 
			_isLoading = value;
			NotifyOfPropertyChange();
		}
	}
}
