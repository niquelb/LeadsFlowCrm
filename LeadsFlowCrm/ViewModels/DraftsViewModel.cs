using Caliburn.Micro;
using Google.Apis.PeopleService.v1.Data;
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

public class DraftsViewModel : Screen
{
	private readonly IGmailServiceClass _gmailService;
	private readonly IEventAggregator _event;

	public DraftsViewModel(IGmailServiceClass gmailService,
						IEventAggregator @event)
    {
		_gmailService = gmailService;
		_event = @event;
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		await LoadDraftsAsync();
	}

	#region Public Methods

	/// <summary>
	/// Method for refreshing the drafts
	/// </summary>
	public async void RefreshDrafts()
	{
		await LoadDraftsAsync();
	}

	/// <summary>
	/// Method for opening a draft
	/// </summary>
	/// <param name="email">Selected draft</param>
	public async void OpenDraft(Email email)
	{
		await PublishSelectedEmailAsync(email);
	}

	/// <summary>
	/// Method for sending a draft>
	/// </summary>
	/// <param name="email">Selected draft</param>
	public async void Send(Email email)
	{
		try
		{
			Utilities.ShowNotification("Sending draft", $"Sending the selected draft to {email.To}...", NotificationType.Information);
			await _gmailService.SendDraftAsync(email);
			Drafts.Remove(email);
			Utilities.ShowNotification("Draft sent successfully", $"The draft was sent to {email.To}.", NotificationType.Success);
		}
		catch (Exception ex)
		{
			Utilities.ShowNotification("Error sending the draft", $"There was an error sending the selected draft to {email.To}. Please try again later. ({ex.Message})", NotificationType.Error);
		}
	}

	/// <summary>
	/// Method for discarding a draft
	/// </summary>
	/// <param name="email">Selected draft</param>
	public async void Discard(Email email)
	{
		try
		{
			Utilities.ShowNotification("Deleting draft", $"Deleting the selected draft...", NotificationType.Information);
			await _gmailService.DeleteDraftAsync(email);
			Drafts.Remove(email);
			Utilities.ShowNotification("Draft deletion successfully", $"The draft to {email.To} was deleted successfully.", NotificationType.Success);
		}
		catch (Exception ex)
		{
			Utilities.ShowNotification("Error deleting the draft", $"There was an error deleting the selected draft. Please try again later. ({ex.Message})", NotificationType.Error);
		}
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

		await LoadDraftsAsync(query: SearchText);
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

	#endregion

	#region Private Methods

	/// <summary>
	/// Method for loading/refreshing the drafts with the optional pagination options
	/// </summary>
	/// <see cref="PaginationOptions"/>
	/// <param name="pagination">Optional pagination options, default is 'PaginationOptions.FirstPage'</param>
	/// <returns></returns>
	private async Task LoadDraftsAsync(PaginationOptions pagination = PaginationOptions.FirstPage,
									string query = "")
	{
		LoadingScreenIsVisible = true;
		ContentIsVisible = false;
		EmptyScreenIsVisible = false;

		Drafts = new(await _gmailService.GetDraftsAsync(query: Utilities.FormatQuery(query)));

		LoadingScreenIsVisible = false;

		if (Drafts.Count > 0)
		{
			ContentIsVisible = true;
		}
		else
		{
			EmptyScreenIsVisible = true;
		}

		CanRefreshDrafts = true;
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

	#endregion

	#region Properties

	/// <summary>
	/// The header for the tab
	/// </summary>
	public string DisplayHeader { get; } = "My Drafts";

    #region Property backing fields

    private bool _loadingScreenIsVisible;
	private bool _emptyScreenIsVisible;
	private bool _contentIsVisible;

	private bool _canRefreshDrafts;
	private BindableCollection<Email> _drafts = new();
	private int _currentPageIndex;
	private string _searchText;

	#endregion

	/// <summary>
	/// Can refresh drafts
	/// </summary>
	public bool CanRefreshDrafts
	{
		get { return _canRefreshDrafts; }
		set { 
			_canRefreshDrafts = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// List of drafts
	/// </summary>
	public BindableCollection<Email> Drafts
	{
		get { return _drafts; }
		set { 
			_drafts = value;
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
	/// Controls the loading screen visibility
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
	/// Controls the empty screen visibility
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
	/// Controls the main content visibility
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
