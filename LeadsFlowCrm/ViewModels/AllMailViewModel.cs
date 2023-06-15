using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using LeadsFlowCrm.Utils;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static LeadsFlowCrm.Services.GmailServiceClass;

namespace LeadsFlowCrm.ViewModels;

public class AllMailViewModel : Screen
{
	private readonly IGmailServiceClass _gmailService;
	private readonly IEventAggregator _event;

	public AllMailViewModel(IGmailServiceClass gmailService,
						 IEventAggregator @event)
    {
		_gmailService = gmailService;
		_event = @event;
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		CurrentPageIndex = 1;
		await LoadEmailsAsync();
	}

	#region Public Methods

	/// <summary>
	/// Method for refreshing the email collection
	/// </summary>
	public async void RefreshEmails()
	{
		CurrentPageIndex = 1;
		//CanPreviousPage = false; TODO <-

		await LoadEmailsAsync();
	}

	/// <summary>
	/// Method for opening an email from the inbox
	/// </summary>
	public async void OpenEmail(Email email)
	{
		await PublishSelectedEmailAsync(email);
	}

	/// <summary>
	/// Method for moving the email to the trash
	/// </summary>
	/// <param name="email">Email to be deleted</param>
	public async void Delete(Email email)
	{
		await _gmailService.MarkEmailAsTrashAsync(email);

		RefreshEmails();
		Utilities.ShowNotification("Email deleted", "Successfully deleted the email.", NotificationType.Success);
	}

	public async void RestoreFromTrash(Email email)
	{
		await _gmailService.MarkEmailAsNotTrashAsync(email);

		RefreshEmails();
		Utilities.ShowNotification("Email recovered", "Successfully recovered the email.", NotificationType.Success);
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
		RefreshEmails();
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Method for loading (or refreshing) the emails
	/// </summary>
	/// <param name="pagination">Optional pagination parameters</param>
	/// <returns></returns>
	private async Task LoadEmailsAsync(PaginationOptions pagination = PaginationOptions.FirstPage)
	{
		LoadingScreenIsVisible = true;
		ContentIsVisible = false;
		EmptyScreenIsVisible = false;

		Emails = new(await _gmailService.GetAllMailAsync(paginationOptions: pagination));

		LoadingScreenIsVisible = false;

		if (Emails.Count > 0)
		{
			ContentIsVisible = true;
		}
		else
		{
			EmptyScreenIsVisible = true;
		}

		// TODO CanRefresh = true
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

	public string DisplayHeader { get; } = "All Mail";

	#region Private backing fields

	private bool _loadingScreenIsVisible;
	private bool _emptyScreenIsVisible;
	private bool _contentIsVisible;

	private BindableCollection<Email> _emails = new();
	private int _currentPageIndex = 1;

	#endregion

	/// <summary>
	/// Emails to be displayed
	/// </summary>
	public BindableCollection<Email> Emails
	{
		get { return _emails; }
		set { 
			_emails = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Index for the pagination
	/// </summary>
	public int CurrentPageIndex
	{
		get { return _currentPageIndex; }
		set { 
			_currentPageIndex = value;
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
		set
		{
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
		set
		{
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
		set
		{
			_contentIsVisible = value;
			NotifyOfPropertyChange();
		}
	}

	#endregion

	#endregion

}
