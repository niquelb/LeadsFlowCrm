using Caliburn.Micro;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using LeadsFlowCrm.Services.ModelServices;
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
		catch (Exception)
		{
			// TODO handle error
			throw;
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

	public async void SaveContact()
	{
		await _contactService.PostToApiAsync(contact: SelectedContact, UserId: _loggedInUser.Id);
	}

	public string DisplayHeader { get; set; } = "Import Contacts";

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
}
