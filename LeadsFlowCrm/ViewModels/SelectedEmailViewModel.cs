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

	public SelectedEmailViewModel(IGmailServiceClass gmailServiceClass, IEventAggregator @event)
    {
		_gmailServiceClass = gmailServiceClass;
		_event = @event;
	}

    protected async override void OnViewLoaded(object view)
	{
		base.OnViewLoaded(view);

		ShowLoadingScreen("Fetching requested email...");

		// We retrieve the selected email
		Email email = _gmailServiceClass.SelectedEmail ?? new();

		// We parse and decode the body, this also populates the "EncodedBody" field
		email.Body = await Task.FromResult(_gmailServiceClass.GetProcessedBody(email));

		// We mark the email as read
		await _gmailServiceClass.MarkEmailAsReadAsync(email);

		await Task.Delay(200);

		SelectedEmail = email;

		// We render the body
		Body = email.Body;

		IsLoading = false;
	}

	protected async override Task OnDeactivateAsync(bool close, CancellationToken cancellationToken)
	{
		await base.OnDeactivateAsync(close, cancellationToken);

		// This is just to avoid a weird UI glitch
		ShowLoadingScreen("");
		await Task.Delay(50);

		_gmailServiceClass.SelectedEmail = null;
	}

	/// <summary>
	/// Method for displaying the loading screen in the view
	/// </summary>
	/// <param name="loadingText">Text that will appear below the loading screen</param>
	private void ShowLoadingScreen(string loadingText)
	{
		LoadingText = loadingText;
		IsLoading = true;
	}

	/// <summary>
	/// Method that will redirect to the inbox
	/// </summary>
	public async void Back() => await _event.PublishOnUIThreadAsync(new NavigationEvent(NavigationEvent.NavigationRoutes.Inbox));

	/// <summary>
	/// Method for marking the email as unread
	/// </summary>
	public async void MarkUnread() => await _gmailServiceClass.MarkEmailAsUnreadAsync(SelectedEmail);

	/*
	 * Property backing fields
	 */
	private Email _selectedEmail = new();
	private string _body = "Loading email body...";
	private string _loadingText;
	private bool _isLoading = true;

	/// <summary>
	/// Selected email
	/// </summary>
	public Email SelectedEmail
	{
		get { return _selectedEmail; }
		set { 
			_selectedEmail = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Email body to be rendered by the WebBrowser control
	/// </summary>
	public string Body
	{
		get { return _body; }
		set { 
			_body = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Represents the text displayed during the loading screen
	/// </summary>
	public string LoadingText
	{
		get { return _loadingText; }
		set { 
			_loadingText = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// This property is used to control the visibility of the loading spinner
	/// </summary>
	public bool IsLoading
	{
		get { return _isLoading; }
		set { 
			_isLoading = value;
			NotifyOfPropertyChange();
		}
	}
}
