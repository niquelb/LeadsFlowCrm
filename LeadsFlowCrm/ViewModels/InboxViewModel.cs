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
	private ObservableCollection<Email> _emails;

    public InboxViewModel(IGmailServiceClass gmailService)
    {
		_gmailService = gmailService;
	}

	protected override async void OnViewLoaded(object view)
	{
		base.OnViewLoaded(view);

		Emails = new ObservableCollection<Email>(await _gmailService.GetEmailsFromInboxAsync());
	}

	public ObservableCollection<Email> Emails
	{
		get { return _emails; }
		set {
			_emails = value;
			NotifyOfPropertyChange();
		}
	}

}
