using Caliburn.Micro;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class DraftsViewModel : Screen
{
	private readonly IGmailServiceClass _gmailService;

	public DraftsViewModel(IGmailServiceClass gmailService)
    {
		_gmailService = gmailService;
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		LoadingScreenIsVisible = true;

		Drafts = new(await _gmailService.GetDraftsAsync());

		LoadingScreenIsVisible = false;

        if (Drafts.Count <= 0)
        {
			EmptyScreenIsVisible = true;
			return;
        }

        ContentIsVisible = true;
	}

	#region Public Methods

	public void OpenDraft(Email email)
	{
		Trace.WriteLine(email.To, "EMAIL");
	}

	#endregion

	#region Private Methods

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

	private BindableCollection<Email> _drafts = new();
	private int _currentPageIndex;

	#endregion

	/// <summary>
	/// Current page index for the paginator
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
