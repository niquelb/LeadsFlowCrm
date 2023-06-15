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
using System.Diagnostics;
using Google.Apis.Gmail.v1;
using Org.BouncyCastle.Cms;
using LeadsFlowCrm.EventModels;

namespace LeadsFlowCrm.ViewModels;

public class CreateDraftViewModel : Screen, IHandle<DraftEvent>
{

	private readonly IGmailServiceClass _gmailService;
	private readonly IContactService _contactService;
	private readonly LoggedInUser _loggedInUser;

	public CreateDraftViewModel(IGmailServiceClass gmailService,
							 IContactService contactService,
							 IEventAggregator @event,
							 LoggedInUser loggedInUser)
	{
		_gmailService = gmailService;
		_contactService = contactService;
		_loggedInUser = loggedInUser;

		@event.SubscribeOnUIThread(this);
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

	#region Public Methods
	/// <summary>
	/// Method to send the email(s)
	/// </summary>
	public async void Send()
	{
		#region null and empty checks
		if (string.IsNullOrWhiteSpace(SubjectLine) || string.IsNullOrWhiteSpace(Body))
		{
			Utilities.ShowNotification("Fields are empty", "The email was not sent because one or more required fields are empty.", NotificationType.Error);
			return;
		}

		if (Recipients.Count <= 0)
		{
			Utilities.ShowNotification("No recipients specified", "The email was not sent because there are no specified recipients.", NotificationType.Error);
			return;
		}
		#endregion

		// We display a notification that the emails are being sent
		if (Recipients.Count > 1)
		{
			Utilities.ShowNotification("Sending emails", $"{Recipients.Count} emails are being sent.", NotificationType.Information);
		}
		else
		{
			Utilities.ShowNotification("Sending email", $"Sending email to {Recipients.FirstOrDefault()}.", NotificationType.Information);
		}

		// We iterate over the recipients and send each one the email
		foreach (var r in Recipients)
		{
			try
			{
				Email email = new()
				{
					To = r,
					SubjectLine = SubjectLine,
					Body = Body,
				};

				await _gmailService.SendEmailAsync(email);

			}
			catch (Exception ex)
			{
				//TODO: don't halt the whole correspondence, also save a log of which emails were not sent
				//TODO: remove specific exception msg for production
				Utilities.ShowNotification("Error", $"Email was not sent. {ex.Message}", NotificationType.Error);

				return;
			}
		}

		// We display a notification that the emails were sent successfully
		if (Recipients.Count > 1)
		{
			Utilities.ShowNotification("Emails sent successfully", $"{Recipients.Count} emails sent successfully.", NotificationType.Success);
		}
		else
		{
			Utilities.ShowNotification("Email sent successfully", $"The email has been sent to {Recipients.FirstOrDefault()}.", NotificationType.Success);
		}

		Clear();
	}

	/// <summary>
	/// Method for saving the draft(s)
	/// </summary>
	public async void SaveDraft()
	{
		#region null and empty checks
		if (string.IsNullOrWhiteSpace(SubjectLine) || string.IsNullOrWhiteSpace(Body))
		{
			Utilities.ShowNotification("Fields are empty", "The draft was not saved because one or more required fields are empty.", NotificationType.Error);
			return;
		}

		if (Recipients.Count <= 0)
		{
			Utilities.ShowNotification("No recipients specified", "The draft was not saved because there are no specified recipients.", NotificationType.Error);
			return;
		}
		#endregion

		// We display a notification that the drafts are being saved
		if (Recipients.Count > 1)
		{
			Utilities.ShowNotification("Saving drafts", $"{Recipients.Count} drafts are being saved.", NotificationType.Information);
		}
		else
		{
			Utilities.ShowNotification("Saving draft", $"Saving draft for {Recipients.FirstOrDefault()}.", NotificationType.Information);
		}

		// We iterate over the recipients and save each draft
		foreach (var r in Recipients)
		{
			try
			{
				Email email = new()
				{
					To = r,
					SubjectLine = SubjectLine,
					Body = Body,
				};

				await _gmailService.SaveDraftAsync(email);

			}
			catch (Exception ex)
			{
				//TODO: don't halt the whole correspondence, also save a log of which drafts were not saved
				//TODO: remove specific exception msg for production
				Utilities.ShowNotification("Error", $"Draft was not saved. {ex.Message}", NotificationType.Error);

				return;
			}
		}

		// We display a notification that the drafts were saved successfully
		if (Recipients.Count > 1)
		{
			Utilities.ShowNotification("Drafts saved successfully", $"{Recipients.Count} drafts were saved successfully.", NotificationType.Success);
		}
		else
		{
			Utilities.ShowNotification("Draft saved successfully", $"The draft for {Recipients.FirstOrDefault()} has been saved successfully.", NotificationType.Success);
		}

		Clear();
	}

	/// <summary>
	/// Method for clearing the view and exiting
	/// </summary>
	public void Discard()
	{
		Clear();

		Utilities.ShowNotification("Draft discarded", "The draft was discarded", NotificationType.Information);

		Exit();
	}

	/// <summary>
	/// Window close function
	/// </summary>
	public async void Exit()
	{
		await TryCloseAsync();
	}

	/// <summary>
	/// Method to add the recipient to the list
	/// </summary>
	public void AddRecipient()
	{
		#region email validation and trimming
		if (string.IsNullOrWhiteSpace(To))
		{
			return;
		}

		//TODO in-depth regex email validator

		// We remove ALL blank space characters
		var recipient = Regex.Replace(To, @"\s", "");

		// We empty the TextBlock
		To = string.Empty;

		// We check for duplicates
		foreach (var r in Recipients)
		{
			if (recipient.Equals(r))
			{
				return;
			}
		}
		#endregion

		Recipients.Add(recipient);
		SelectedRecipient = recipient;

		IsRecipientSelected = true;
	}
	#endregion

	#region Private Methods

	/// <summary>
	/// Method for clearing the view
	/// </summary>
	private void Clear()
	{
		To = string.Empty;
		SubjectLine = string.Empty;
		Body = string.Empty;
		Recipients.Clear();
		SelectedRecipient = string.Empty;

		IsRecipientSelected = false;
	}

	#endregion

	#region Events

	/// <summary>
	/// This is an event that gets fired when the user wants to create a new draft with a given list of emails (strings) as recipients
	/// </summary>
	/// <see cref="DraftEvent"/>
	/// <param name="e">DraftEvent object</param>
	/// <param name="cancellationToken">Cancellation token</param>
	/// <returns></returns>
	public async Task HandleAsync(DraftEvent e, CancellationToken cancellationToken)
	{
        if (e.Recipients.Count <= 0)
        {
			return;
        }

		/*
		 * We add each email address as a recipient
		 */
        foreach (var r in e.Recipients)
        {
			To = r; // ← We do this since that's how the AddRecipients method is setup to work, by reading the Propery set by the user

			AddRecipient();
		}

		// We clear the "To" property
		To = string.Empty;
    }

	#endregion

	#region Properties
	/// <summary> 
	/// Contacts that "belong" to the user (referencing the "UserId" field) from the API.
	/// </summary>
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
		set
		{
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
		set
		{
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
		set
		{
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
		set
		{
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
		set
		{
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
		set
		{
			_selectedRecipient = value;
			NotifyOfPropertyChange();
		}
	}
	#endregion

}
