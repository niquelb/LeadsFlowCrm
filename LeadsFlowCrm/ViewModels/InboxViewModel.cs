﻿using Caliburn.Micro;
using LeadsFlowCrm.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class InboxViewModel : Screen
{
	private ObservableCollection<Email> _emails = new();

	public ObservableCollection<Email> Emails
	{
		get { return _emails; }
		set {
			_emails = value;
			NotifyOfPropertyChange();
		}
	}

	public InboxViewModel()
	{
		Emails.Add(new Email
		{
			Sender = "Mike",
			SubjectLine = "Business Oportunity",
			Body = "This is some really long text",
			IsFavorite = true,
			IsRead = true,
		});
	}

}
