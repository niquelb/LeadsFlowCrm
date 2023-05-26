using Caliburn.Micro;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class SelectedEmailViewModel : Screen
{
	private readonly IGmailServiceClass _gmailServiceClass;

	public SelectedEmailViewModel(IGmailServiceClass gmailServiceClass)
    {
		_gmailServiceClass = gmailServiceClass;
	}

    protected async override void OnViewLoaded(object view)
	{
		base.OnViewLoaded(view);

		SelectedEmail = _gmailServiceClass.SelectedEmail ?? new();

		//TODO: load email body
		await Task.Delay(500);

		IsLoading = false;
	}

	private Email _selectedEmail;

	public Email SelectedEmail
	{
		get { return _selectedEmail; }
		set { 
			_selectedEmail = value;
			NotifyOfPropertyChange();
		}
	}


	/*
	 * This property is used to control the visibility of the loading spinner and the actual email
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
}
