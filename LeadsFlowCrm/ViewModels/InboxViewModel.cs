using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class InboxViewModel : Screen
{
	private readonly IGmailServiceClass _gmailService;
	private readonly IEventAggregator _event;

	public InboxViewModel(IGmailServiceClass gmailService, IEventAggregator @event)
	{
		_gmailService = gmailService;
		_event = @event;
	}

	protected async override void OnViewLoaded(object view)
	{
		base.OnViewLoaded(view);

		Inbox = new ObservableCollection<Email>(await _gmailService.GetInboxAsync());

		IsLoading = false;
		CanRefreshInbox = true;
	}

	public async void RefreshInbox()
	{
		IsLoading = true;
		CanRefreshInbox = false;

		Inbox = new ObservableCollection<Email>(await _gmailService.RefreshAndGetInboxAsync());

		IsLoading = false;
		CanRefreshInbox = true;
	}

	/// <summary>
	/// Event that gets triggered when the user selects an email
	/// </summary>
	/// <param name="email">Selected email</param>
	/// <returns></returns>
	private async Task PublishSelectedEmailAsync(Email email)
	{
		_gmailService.SelectedEmail = email;

		await _event.PublishOnUIThreadAsync(new EmailSelectedEvent());
	}

	/*
	 * Property backing fields
	 */
	private bool _isLoading = true;
	private bool _canRefreshInbox = false;
	private Email _selectedEmail;
	private ObservableCollection<Email> _inbox = new();

	/// <summary>
	/// This property is used to control the visibility of the loading spinner
	/// </summary>
	public bool IsLoading
	{
		get { return _isLoading; }
		set
		{
			_isLoading = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Property to control if the refresh button is enabled or not
	/// </summary>
	public bool CanRefreshInbox
	{
		get { return _canRefreshInbox; }
		set
		{
			_canRefreshInbox = value;
			NotifyOfPropertyChange();
		}
	}


	/// <summary>
	/// Selected email
	/// </summary>
	public Email SelectedEmail
	{
		get { return _selectedEmail; }
		set
		{
			_selectedEmail = value;
			NotifyOfPropertyChange();

			/*
			 * We notify the UI that the user has selected an email.
			 * 
			 * We have to wrap this in an if because when the user refreshes the inbox this also gets triggered
			 * with a null value, breaking the app.
			 */
			if (_selectedEmail != null)
			{
				PublishSelectedEmailAsync(_selectedEmail);
			}
		}
	}

	/// <summary>
	/// List of emails from the user's inbox that will be displayed
	/// </summary>
	public ObservableCollection<Email> Inbox
	{
		get { return _inbox; }
		set
		{
			_inbox = value;
			NotifyOfPropertyChange();
		}
	}
}
