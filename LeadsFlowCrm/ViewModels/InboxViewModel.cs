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

	private ObservableCollection<Email> _inbox = new();
	private Email _selectedEmail;

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
	}

	/*
	 * This property is used to control the visibility of the loading spinner and the inbox ListView
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

	/// <summary>
	/// Selected email
	/// </summary>
	public Email SelectedEmail
	{
		get { return _selectedEmail; }
		set { 
			_selectedEmail = value; 
			NotifyOfPropertyChange();

			/*
			 * We notify the UI that the user has selected an email.
			 */
			PublishSelectedEmailAsync(_selectedEmail);
		}
	}

	private async Task PublishSelectedEmailAsync(Email email)
	{
		_gmailService.SelectedEmail = email;

		await _event.PublishOnUIThreadAsync(new EmailSelectedEvent());
	}

	/// <summary>
	/// List of emails from the user's inbox that will be displayed
	/// </summary>
	public ObservableCollection<Email> Inbox
	{
		get { return _inbox; }
		set {
			_inbox = value;
			NotifyOfPropertyChange();
		}
	}
}
