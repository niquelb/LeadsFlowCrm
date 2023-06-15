using Caliburn.Micro;
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

public class AllMailViewModel : Screen
{
	private readonly IGmailServiceClass _gmailService;

	public AllMailViewModel(IGmailServiceClass gmailService)
    {
		_gmailService = gmailService;
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		CurrentPageIndex = 1;
		await LoadEmailsAsync();
	}

	#region Public Methods

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

		Mail = new BindableCollection<Email>(await _gmailService.GetAllMailAsync(paginationOptions: pagination));

		LoadingScreenIsVisible = false;

		if (Mail.Count > 0)
		{
			ContentIsVisible = true;
		}
		else
		{
			EmptyScreenIsVisible = true;
		}

		// TODO CanRefresh = true
	}

	#endregion

	#region Properties

	public string DisplayHeader { get; } = "All Mail";

	#region Private backing fields

	private bool _loadingScreenIsVisible;
	private bool _emptyScreenIsVisible;
	private bool _contentIsVisible;

	private BindableCollection<Email> _mail;
	private int _currentPageIndex = 1;

	#endregion

	/// <summary>
	/// Emails to be displayed
	/// </summary>
	public BindableCollection<Email> Mail
	{
		get { return _mail; }
		set { 
			_mail = value;
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
