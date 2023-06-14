using Caliburn.Micro;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using LeadsFlowCrm.Services.ModelServices;
using LeadsFlowCrm.Utils;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class ImportContactsViewModel : Screen
{
	private readonly IContactService _contactService;
	private readonly IPeopleServiceClass _peopleService;
	private readonly LoggedInUser _loggedInUser;

	public ImportContactsViewModel(IContactService contactService,
								IPeopleServiceClass peopleService,
								LoggedInUser loggedInUser)
    {
		_contactService = contactService;
		_peopleService = peopleService;
		_loggedInUser = loggedInUser;
	}


	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		ShowLoadingScreen = true;
		NoneSelected = true;

		try
		{
			// We retrieve the contacts from the People API
			GoogleContacts = new(await _contactService.GetFromPeopleApiAsync());
		}
		catch (Exception ex)
		{
			Utilities.ShowNotification("Error loading contacts", $"There was an error loading the contacts ({ex.Message})", NotificationType.Error);
			return;
		}

		ShowLoadingScreen = false;

		// If no contacts found, we show the "empty" screen
		if (GoogleContacts.Count <= 0)
		{
			ShowEmptyScreen = true;
		}
		else
		{
			ShowContent = true;
		}
	}

	#region Public Methods
	/// <summary>
	/// Method for saving the selected contact into the API (Contact table)
	/// </summary>
	public async void SaveContact()
	{
		if (await CheckIfExists(SelectedContact.Email))
		{
			Utilities.ShowNotification("Contact already exists", $"A contact with this email address already exists. ({SelectedContact.Email})", NotificationType.Error);
			return;
		}

		try
		{
			await _contactService.PostToApiAsync(contact: SelectedContact, UserId: _loggedInUser.Id);
			Utilities.ShowNotification("Success", $"Contact ({SelectedContact.Email}) saved successfully.", NotificationType.Success);
		}
		catch (Exception ex)
		{
			Utilities.ShowNotification("Error saving contact", $"There was an error saving the selected contact ({ex.Message})", NotificationType.Error);
		}
	}
	#endregion

	#region Private Methods

	/// <summary>
	/// Method for checking if the contact exists based on a given email address
	/// </summary>
	/// <param name="email">Email address</param>
	/// <returns>True if the contact exists, false if not</returns>
	/// <exception cref="ArgumentNullException">If the email is null/empty</exception>
	private async Task<bool> CheckIfExists(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
		{
			throw new ArgumentNullException(nameof(email));
		}

		Contact? contact = await _contactService.GetByEmailAsync(email);

		return contact != null;
	}

	#endregion

	#region Properties
	/// <summary>
	/// Tab name for the view
	/// </summary>
	public string DisplayHeader { get; } = "Import Contacts";

	#region Private backing fields

	private bool _showContent;
	private bool _showEmptyScreen;
	private bool _showLoadingScreen;
	private bool _noneSelected = true;
	private BindableCollection<Contact> _googleContacts = new();
	private Contact _selectedContact = new();

	#endregion

	/// <summary>
	/// Contacts
	/// </summary>
	public BindableCollection<Contact> GoogleContacts
	{
		get { return _googleContacts; }
		set
		{
			_googleContacts = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Selected contact
	/// </summary>
	public Contact SelectedContact
	{
		get { return _selectedContact; }
		set
		{
			_selectedContact = value;
			NotifyOfPropertyChange();

			// We display the contact info
			NoneSelected = false;
		}
	}

	#region Visibility properties

	/// <summary>
	/// Controls wether the selected contact info is displayed or not
	/// </summary>
	public bool NoneSelected
	{
		get { return _noneSelected; }
		set
		{
			_noneSelected = value;
			NotifyOfPropertyChange();
		}
	}


	/// <summary>
	/// Controls wether the main content is showing or not
	/// </summary>
	public bool ShowContent
	{
		get { return _showContent; }
		set
		{
			_showContent = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls wether the loading screen is showing or not
	/// </summary>
	public bool ShowLoadingScreen
	{
		get { return _showLoadingScreen; }
		set
		{
			_showLoadingScreen = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls wether the "empty" screen is showing or not
	/// </summary>
	public bool ShowEmptyScreen
	{
		get { return _showEmptyScreen; }
		set
		{
			_showEmptyScreen = value;
			NotifyOfPropertyChange();
		}
	}

	#endregion

	#endregion
}
