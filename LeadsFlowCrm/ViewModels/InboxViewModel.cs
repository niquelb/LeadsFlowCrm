using Caliburn.Micro;
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

	private ObservableCollection<Email> _inbox = new();

    public InboxViewModel(IGmailServiceClass gmailService)
    {
		_gmailService = gmailService;
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

	public ObservableCollection<Email> Inbox
	{
		get { return _inbox; }
		set {
			_inbox = value;
			NotifyOfPropertyChange();
		}
	}
}
