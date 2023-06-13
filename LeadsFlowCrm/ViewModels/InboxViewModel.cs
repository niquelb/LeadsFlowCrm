using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LeadsFlowCrm.Services.GmailServiceClass;

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

	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		/*
		 * We clear the selected item so that the same email can be opened twice in a row.
		 * We don't use the property because that fires an event, instead we modify the backing field.
		 */
		_selectedEmail = new();
	}

	protected async override void OnViewLoaded(object view)
	{
		base.OnViewLoaded(view);

		Inbox = new ObservableCollection<Email>(await _gmailService.GetInboxAsync());

		IsLoading = false;
		CanRefreshInbox = true;

		IsInboxEmpty = Inbox.Count <= 0;
	}

	#region Public Methods
	/// <summary>
	/// Method for refreshing the inbox
	/// </summary>
	public async void RefreshInbox()
	{
		CurrentPageIndex = 1;
		CanPreviousPage = false;

		await LoadInbox();
	}

	#region Pagination
	/// <summary>
	/// Method for updating the inbox with the next page
	/// </summary>
	public async void NextPage()
	{
		CurrentPageIndex++;
		CanPreviousPage = true;

		await LoadInbox(PaginationOptions.NextPage);
	}

	/// <summary>
	/// Method for updating the inbox with the next page
	/// </summary>
	public async void PreviousPage()
	{
        if (CurrentPageIndex < 1)
        {
			return;
        }

		CurrentPageIndex--;
		CanPreviousPage = CurrentPageIndex != 1;

		await LoadInbox(PaginationOptions.PreviousPage);
	}
	#endregion

	/// <summary>
	/// Method for opening an email from the inbox
	/// </summary>
	public async void OpenEmail()
	{
		if (_selectedEmail == null)
		{
			return;
		}

		await PublishSelectedEmailAsync(_selectedEmail);
	}
	#endregion

	#region Private Methods
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

	/// <summary>
	/// Method for loading/refreshing the inbox with the optional pagination options
	/// </summary>
	/// <see cref="PaginationOptions"/>
	/// <param name="pagination">Optional pagination options, default is 'PaginationOptions.FirstPage'</param>
	/// <returns></returns>
	private async Task LoadInbox(PaginationOptions pagination = PaginationOptions.FirstPage)
	{
		IsInboxEmpty = false;
		IsLoading = true;
		CanRefreshInbox = false;

		Inbox = new ObservableCollection<Email>(await _gmailService.GetPaginatedInbox(pagination));

		IsLoading = false;
		CanRefreshInbox = true;

		IsInboxEmpty = Inbox.Count <= 0;

	}
	#endregion

	#region Properties

	#region Property backing fields
	private bool _isLoading = true;
	private bool _isInboxEmpty;
	private bool _canRefreshInbox;
	private bool _canPreviousPage;
	private Email? _selectedEmail;
	private ObservableCollection<Email> _inbox = new();
	private int _currentPageIndex = 1;
	#endregion

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
	/// This property controls wether the "empty" screen is showing or not
	/// </summary>
	public bool IsInboxEmpty
	{
		get { return _isInboxEmpty; }
		set { 
			_isInboxEmpty = value;
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
	/// Reflects wether or not the user can go to the previous page
	/// </summary>
	public bool CanPreviousPage
	{
		get { return _canPreviousPage; }
		set { 
			_canPreviousPage = value;
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

	/// <summary>
	/// Index of the current page for the paginator
	/// </summary>
	public int CurrentPageIndex
	{
		get { return _currentPageIndex; }
		set {
			_currentPageIndex = value;
			NotifyOfPropertyChange();
		}
	}

	#endregion
}
