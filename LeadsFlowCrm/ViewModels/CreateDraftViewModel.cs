using Caliburn.Micro;
using GmailApi = Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeadsFlowCrm.Services;
using LeadsFlowCrm.Models;
using System.Diagnostics;
using System.Windows.Documents;
using System.Windows.Controls;

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
		if (string.IsNullOrWhiteSpace(To) || string.IsNullOrWhiteSpace(SubjectLine) || string.IsNullOrWhiteSpace(Body))
		{
			return;
		}

		try
		{
			Email email = new()
			{
				To = To,
				SubjectLine = SubjectLine,
				Body = Body,
			};

			await _gmailService.SendEmailAsync(email);

			Exit();
		}
		catch (Exception ex)
		{
			Trace.TraceError(ex.Message);
		}
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
