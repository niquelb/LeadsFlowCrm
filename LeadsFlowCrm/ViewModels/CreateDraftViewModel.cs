using Caliburn.Micro;
using GmailApi = Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadsFlowCrm.Services;
using LeadsFlowCrm.Models;

namespace LeadsFlowCrm.ViewModels;

public class CreateDraftViewModel : Screen
{

	private readonly IGmailServiceClass _gmailService;

    public CreateDraftViewModel(IGmailServiceClass gmailService)
    {
		_gmailService = gmailService;
	}

	public async void Send()
	{
		// TEST CODE
		var email = new Email()
		{
			To = "nicolas.palaorocort03@gmail.com",
			SubjectLine = "Test email",
			Body = "Lorem ipsum or summ like that"
		};

		await _gmailService.SendEmailAsync(email);
	}

    public async void Exit()
	{
		await TryCloseAsync();
	}

	/*
	 * Prop backing fields
	 */
	private string _to = string.Empty;
	private string _subjectLine = string.Empty;
	private string _body = string.Empty;

	/// <summary> Message that will be sent/drafted </summary>
	public GmailApi.Message Msg { get; set; } = new();

	/// <summary>
	/// Reciever
	/// </summary>
	public string To
	{
		get { return _to; }
		set { 
			_to = value;
			NotifyOfPropertyChange();
		}
	}
	
	/// <summary>
	/// Subject line
	/// </summary>
	public string SubjectLine
	{
		get { return _subjectLine; }
		set { 
			_subjectLine = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Body
	/// </summary>
	public string Body
	{
		get { return _body; }
		set { 
			_body = value;
			NotifyOfPropertyChange();
		}
	}

}
