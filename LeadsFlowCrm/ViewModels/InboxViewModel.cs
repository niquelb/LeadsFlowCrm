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
		Inbox = new ObservableCollection<Email>();
	}

	protected async override void OnViewLoaded(object view)
	{
		base.OnViewLoaded(view);

		Inbox = new ObservableCollection<Email>(await _gmailService.GetInboxAsync());
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
