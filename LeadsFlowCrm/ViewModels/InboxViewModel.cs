﻿using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using LeadsFlowCrm.Utils;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using static LeadsFlowCrm.Services.GmailServiceClass;

namespace LeadsFlowCrm.ViewModels;

public class InboxViewModel : Screen
{
	private readonly IGmailServiceClass _gmailService;
	private readonly IEventAggregator _event;
	private readonly IWindowManager _windowManager;
	private readonly CreateDraftViewModel _createDraft;

	public InboxViewModel(IGmailServiceClass gmailService,
					   IEventAggregator @event,
					   IWindowManager windowManager,
					   CreateDraftViewModel createDraft)
	{
		_gmailService = gmailService;
		_event = @event;
		_windowManager = windowManager;
		_createDraft = createDraft;
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		CurrentPageIndex = 1;

		await LoadInboxAsync();
	}

	#region Public Methods
	/// <summary>
	/// Method for refreshing the inbox
	/// </summary>
	public async void RefreshInbox()
	{
		CurrentPageIndex = 1;
		CanPreviousPage = false;

		await LoadInboxAsync();
	}

	/// <summary>
	/// Method for creating a draft with the given email's sender as the recipient (not an actual reply in the thread but shh)
	/// </summary>
	/// <param name="email">Selected email</param>
	public async void Reply(Email email)
	{
		string emailAddress = email.FromEmail;

		await _windowManager.ShowWindowAsync(_createDraft);

		await _event.PublishOnUIThreadAsync(new DraftEvent()
		{
			Recipients = new List<string>() {
				emailAddress
			}
		});
	}

	/// <summary>
	/// Method for opening an email from the inbox
	/// </summary>
	public async void OpenEmail(Email email)
	{
		await PublishSelectedEmailAsync(email);
	}

	/// <summary>
	/// Method for moving an email to the trash
	/// </summary>
	/// <param name="email">Email to be deleted</param>
	public async void Delete(Email email)
	{
		await _gmailService.MarkEmailAsTrashAsync(email);

		Inbox.Remove(email);
		Utilities.ShowNotification("Email deleted", "Successfully deleted the email.", NotificationType.Success);
	}

	/// <summary>
	/// Method for archiving the email
	/// </summary>
	/// <param name="email">Email to be archived</param>
	public async void Archive(Email email)
	{
		await _gmailService.MarkEmailAsArchivedAsync(email);

		Inbox.Remove(email);
		Utilities.ShowNotification("Email archived", "Successfully archived the email.", NotificationType.Success);
	}

	/// <summary>
	/// Method for starring/de-starring an email
	/// </summary>
	/// <param name="email">Email to be modified</param>
	public async void MarkFavorite(Email email)
	{
		if (email.IsFavorite)
		{
			await _gmailService.MarkEmailAsNotFavoriteAsync(email);
			email.IsFavorite = false;
			Utilities.ShowNotification("Marked as not favorite", "Successfully marked the email as not favorite.", NotificationType.Success);
		}
		else
		{
			await _gmailService.MarkEmailAsFavoriteAsync(email);
			email.IsFavorite = true;
			Utilities.ShowNotification("Marked as favorite", "Successfully marked the email as favorite.", NotificationType.Success);
		}

		// We refresh the inbox to update the icons
		RefreshInbox();
	}

	/// <summary>
	/// Method to query the inbox
	/// </summary>
	public async void Query()
	{
        if (string.IsNullOrWhiteSpace(SearchText))
        {
			return;
        }

        await LoadInboxAsync(query: SearchText);
	}

	/// <summary>
	/// Method that gets executed when the user presses a key on the search box, if that key is "enter", the query method
	/// is executed
	/// </summary>
	/// <param name="keyArgs">Key arguments</param>
	public void SubmitSearch(KeyEventArgs keyArgs)
	{
		if (keyArgs.Key != Key.Enter)
		{
			return;
		}

		Query();
	}

	#region Pagination
	/// <summary>
	/// Method for updating the inbox with the next page
	/// </summary>
	public async void NextPage()
	{
		CurrentPageIndex++;
		CanPreviousPage = true;

		await LoadInboxAsync(PaginationOptions.NextPage);
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

		await LoadInboxAsync(PaginationOptions.PreviousPage);
	}
	#endregion

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
	private async Task LoadInboxAsync(PaginationOptions pagination = PaginationOptions.FirstPage,
								   string query = "")
	{
		LoadingScreenIsVisible = true;
		ContentIsVisible = false;
		EmptyScreenIsVisible = false;

		Inbox = new ObservableCollection<Email>(await _gmailService.GetInboxAsync(paginationOption: pagination, query: Utilities.FormatQuery(query)));

		SearchText = string.Empty;

		LoadingScreenIsVisible = false;

		if (Inbox.Count > 0)
		{
			ContentIsVisible = true;
		}
		else
		{
			EmptyScreenIsVisible = true;
		}

		CanRefreshInbox = true;
	}

	
	#endregion

	#region Properties

	/// <summary>
	/// Tab name
	/// </summary>
	public string DisplayHeader { get; } = "Inbox";

	#region Property backing fields

	private bool _loadingScreenIsVisible;
	private bool _emptyScreenIsVisible;
	private bool _contentIsVisible;

	private bool _canRefreshInbox;
	private bool _canPreviousPage;
	private ObservableCollection<Email> _inbox = new();
	private int _currentPageIndex = 1;
	private string _searchText = string.Empty;

	#endregion

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

	/// <summary>
	/// Searchbox text
	/// </summary>
	public string SearchText
	{
		get { return _searchText; }
		set {
			_searchText = value;
			NotifyOfPropertyChange();
		}
	}

	#region Visibility controls

	/// <summary>
	/// Controls the visibility of the loading screen
	/// </summary>
	public bool LoadingScreenIsVisible
	{
		get { return _loadingScreenIsVisible; }
		set {
			_loadingScreenIsVisible = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls the visibility of the "empty" screen
	/// </summary>
	public bool EmptyScreenIsVisible
	{
		get { return _emptyScreenIsVisible; }
		set {
			_emptyScreenIsVisible = value; 
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls the visibility of the main content
	/// </summary>
	public bool ContentIsVisible
	{
		get { return _contentIsVisible; }
		set {
			_contentIsVisible = value;
			NotifyOfPropertyChange();
		}
	}


	#endregion

	#endregion
}
