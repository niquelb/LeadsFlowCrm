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
using Notifications.Wpf.Controls;
using Notifications.Wpf;

namespace LeadsFlowCrm.ViewModels;

public class CreateDraftViewModel : Screen
{

	private readonly IGmailServiceClass _gmailService;

    public CreateDraftViewModel(IGmailServiceClass gmailService)
    {
		_gmailService = gmailService;
	}

	/// <summary>
	/// Method for showing a "toast" style notification to the user
	/// </summary>
	/// <param name="title">Notification title</param>
	/// <param name="msg">Description/message</param>
	/// <param name="notificationType">Notification type (eg. success, error...)</param>
	private void ShowNotification(string title, string msg, NotificationType notificationType) =>
		Notification.Show(new NotificationContent
		{
			Title = title,
			Message = msg,
			Type = notificationType
		});

	public async void Send()
	{
		if (string.IsNullOrWhiteSpace(To) || string.IsNullOrWhiteSpace(SubjectLine) || string.IsNullOrWhiteSpace(Body))
		{
			ShowNotification("Fields are empty", "The email was not sent because one or more required fields are empty.", NotificationType.Error);
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

			ShowNotification("Email sent successfully", $"The email has been sent to {To}.", NotificationType.Success);

			Exit();
		}
		catch (Exception ex)
		{
			//TODO: remove specific exception msg for production
			ShowNotification("Error", $"Email was not sent. {ex.Message}", NotificationType.Error);
		}
	}

    public async void Exit()
	{
		To = string.Empty; 
		SubjectLine = string.Empty;
		Body = string.Empty;

		await TryCloseAsync();
	}

	public NotificationManager Notification { get; set; } = new();

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
