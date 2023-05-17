using Caliburn.Micro;
using LeadsFlowCrm.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class InboxViewModel
{
	private BindableCollection<EmailModel> _emails = new();

	public InboxViewModel()
	{
		Emails.Add(new EmailModel
		{
			Sender = "Mike",
			SubjectLine = "Business Oportunity",
			Body = "This is some really long text",
			IsFavorite = true,
			IsRead = true,
		});
	}

	public BindableCollection<EmailModel> Emails
	{
		get { return _emails; }
		set { _emails = value; }
	}

}
