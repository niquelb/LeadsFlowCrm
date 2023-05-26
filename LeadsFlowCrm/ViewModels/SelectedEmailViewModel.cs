using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

	/// <summary>
	/// Method that will redirect to the inbox
	/// </summary>
	public async void Back()
	{
		IsLoading = true;

		_gmailServiceClass.SelectedEmail = null;

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
