using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
using LeadsFlowCrm.Utils;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class MyContactsViewModel : Screen
{
	private readonly IContactService _contactService;
	private readonly LoggedInUser _loggedInUser;

	public MyContactsViewModel(IContactService contactService,
							   LoggedInUser loggedInUser)
    {
		_contactService = contactService;
		_loggedInUser = loggedInUser;
	}

	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		ShowLoadingScreen = true;
		NoneSelected = true;

		// We retrieve the contacts from the API
		Contacts = new BindableCollection<Contact>(await GetContactsAsync());

		ShowLoadingScreen = false;

		// If no contacts found, we show the "empty" screen
		if (Contacts.Count <= 0)
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
	/// Method for deleting the selected contact
	/// </summary>
	public async void Delete()
	{
		try
		{
			//TODO Implement method
		}
		catch (Exception ex)
		{
			Utilities.ShowNotification("Error loading contacts", $"There was an error loading the contacts ({ex.Message})", NotificationType.Error);
			return;
		}
	}
	#endregion

	#region Private Methods
	/// <summary>
	/// Method for retrieving the contacts from the API
	/// </summary>
	/// <returns>List of Contacts</returns>
	private async Task<IList<Contact>> GetContactsAsync()
	{
		try
		{
			return await _contactService.GetByUserAsync(_loggedInUser.Id);
		}
		catch (Exception ex)
		{
			Utilities.ShowNotification("Error loading contacts", $"There was an error loading the contacts ({ex.Message})", NotificationType.Error);
			return new List<Contact>();
		}
	}
	#endregion

	#region Properties
	/// <summary>
	/// Tab name for the view
	/// </summary>
	public string DisplayHeader { get; } = "My Contacts";

	#region Private backing fields

	private bool _showContent;
	private bool _showEmptyScreen;
	private bool _showLoadingScreen;
	private bool _noneSelected = true;
	private BindableCollection<Contact> _contacts = new();
	private Contact _selectedContact = new();

	#endregion

	/// <summary>
	/// Contacts
	/// </summary>
	public BindableCollection<Contact> Contacts
	{
		get { return _contacts; }
		set { 
			_contacts = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Selected contact
	/// </summary>
	public Contact SelectedContact
	{
		get { return _selectedContact; }
		set { 
			_selectedContact = value;
			NotifyOfPropertyChange();

			// We display the contact info
			NoneSelected = false;
		}
	}

	#region Visibility controls

	/// <summary>
	/// Controls wether the selected contact info is displayed or not
	/// </summary>
	public bool NoneSelected
	{
		get { return _noneSelected; }
		set { 
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
		set { 
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
		set { 
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
		set {
			_showEmptyScreen = value;
			NotifyOfPropertyChange();
		}
	}

	#endregion
	#endregion
}
