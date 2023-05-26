using Caliburn.Micro;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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

		// We retrieve the selected email
		Email email = _gmailServiceClass.SelectedEmail ?? new();

		// We parse and decode the body, this also populates the "EncodedBody" field
		email.Body = await Task.FromResult(_gmailServiceClass.GetProcessedBody(email));

		SelectedEmail = email;

		// We render the body
		await Task.Delay(500).ContinueWith((_) => { Body = email.Body; });

		IsLoading = false;
	}

	/*
	 * Selected email
	 */
	private Email _selectedEmail = new();

	public Email SelectedEmail
	{
		get { return _selectedEmail; }
		set { 
			_selectedEmail = value;
			NotifyOfPropertyChange();
		}
	}

	/*
	 * Email body to be rendered by the WebBrowser control
	 */
	private string _body = "Loading email body...";

	public string Body
	{
		get { return _body; }
		set { 
			_body = value;
			NotifyOfPropertyChange();
		}
	}


	/*
	 * This property is used to control the visibility of the loading spinner
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
