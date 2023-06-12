using Caliburn.Micro;
using GmailApi = Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LeadsFlowCrm.Services;
using LeadsFlowCrm.Models;
using Notifications.Wpf;
using System.Threading;
using LeadsFlowCrm.Services.ModelServices;
using LeadsFlowCrm.Utils;
using System.Text.RegularExpressions;

namespace LeadsFlowCrm.ViewModels;

public class CreateDraftViewModel : Screen
{

	private readonly IGmailServiceClass _gmailService;
	private readonly IContactService _contactService;
	private readonly LoggedInUser _loggedInUser;

	public CreateDraftViewModel(IGmailServiceClass gmailService,
							 IContactService contactService,
							 LoggedInUser loggedInUser)
    {
		_gmailService = gmailService;
		_contactService = contactService;
		_loggedInUser = loggedInUser;
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		// We retrieve the contacts from the API
		Contacts = (await _contactService.GetByUserAsync(_loggedInUser.Id)).ToList();

		/*
		 * We iterate through these contacts and add the email to the AutoCompleteData collection.
		 * 
		 * This allows for the autocompletion of the TextBox with the contact's emails.
		 */
        foreach (var contact in Contacts)
        {
			AutoCompleteData.Add(contact.Email);
        }
    }

	

	/// <summary>
	/// Method to send the email
	/// </summary>
	public async void Send()
	{
		if (string.IsNullOrWhiteSpace(To) || string.IsNullOrWhiteSpace(SubjectLine) || string.IsNullOrWhiteSpace(Body))
		{
			Utilities.ShowNotification("Fields are empty", "The email was not sent because one or more required fields are empty.", NotificationType.Error);
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

			Utilities.ShowNotification("Email sent successfully", $"The email has been sent to {To}.", NotificationType.Success);

			Exit();
		}
		catch (Exception ex)
		{
			//TODO: remove specific exception msg for production
			Utilities.ShowNotification("Error", $"Email was not sent. {ex.Message}", NotificationType.Error);
		}
	}

	/// <summary>
	/// Window close function
	/// </summary>
    public async void Exit()
	{
		To = string.Empty; 
		SubjectLine = string.Empty;
		Body = string.Empty;

		await TryCloseAsync();
	}

	public void AddRecipient()
	{
        if (string.IsNullOrWhiteSpace(To))
        {
			return;   
        }

		//TODO in-depth regex email validator

		// We remove ALL blank space characters
		var recipient = Regex.Replace(To, @"\s", "");
		To = string.Empty;

		// We check for duplicates
		foreach (var r in Recipients)
        {
            if (recipient.Equals(r))
            {
				return;
            }
        }

        Recipients.Add(recipient);
		SelectedRecipient = recipient;

		IsRecipientSelected = true;
	}

	/// <summary> Message that will be sent/drafted </summary>
	public GmailApi.Message Msg { get; set; } = new();

	/// <summary> Contacts that "belong" to the user from the API </summary>
	public List<Contact> Contacts { get; set; } = new();

	#region Prop backing fields

	private string _to = string.Empty;
	private string _subjectLine = string.Empty;
	private string _body = string.Empty;
	private BindableCollection<string> _autoCompleteData = new();
	private BindableCollection<string> _recipients = new();
	private bool _isRecipientSelected;
	private string _selectedRecipient = string.Empty;

	#endregion

	/// <summary>
	/// Controls wether or not the recipients combobox is visible
	/// </summary>
	public bool IsRecipientSelected
	{
		get { return _isRecipientSelected; }
		set
		{
			_isRecipientSelected = value;
			NotifyOfPropertyChange();
		}
	}

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

	/// <summary>
	/// A listto be used for the autofill of the "to" field
	/// </summary>
	public BindableCollection<string> AutoCompleteData
	{
		get { return _autoCompleteData; }
		set { 
			_autoCompleteData = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Collection of email addresses that the email will be sent to
	/// </summary>
	public BindableCollection<string> Recipients
	{
		get { return _recipients; }
		set { 
			_recipients = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Selected item from the Recipients collection
	/// </summary>
	public string SelectedRecipient
	{
		get { return _selectedRecipient; }
		set { 
			_selectedRecipient = value;
			NotifyOfPropertyChange();
		}
	}


}
