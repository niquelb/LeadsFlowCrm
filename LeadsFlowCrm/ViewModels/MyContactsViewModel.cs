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

		await LoadContacts();
	}

	#region Public Methods
	/// <summary>
	/// Method for deleting the selected contact
	/// </summary>
	public async void Delete()
	{
		//TODO add confirmation dialog
		try
		{
			await _contactService.DeleteFromApiAsync(SelectedContact);

			Utilities.ShowNotification("Contact deleted successfully", $"The contact was deleted successfully.", NotificationType.Success);

			await LoadContacts();
		}
		catch (Exception ex)
		{
			Utilities.ShowNotification("Error deleting contact", $"There was an error deleting the selected contact. ({ex.Message})", NotificationType.Error);
			return;
		}
	}
	#endregion

	#region Private Methods

	/// <summary>
	/// Method for loading the contacts and initializing/resetting the view
	/// </summary>
	/// <returns></returns>
	private async Task LoadContacts()
	{
		ContentIsVisible = false;
		LoadingScreenIsVisible = true;

		// We retrieve the contacts from the API
		Contacts = new BindableCollection<Contact>(await GetContactsAsync());

		SelectedContactIsVisible = false;
		NoneSelectedIsVisible = true;

		LoadingScreenIsVisible = false;

		// If no contacts found, we show the "empty" screen
		if (Contacts.Count <= 0)
		{
			EmptyScreenIsVisible = true;
		}
		else
		{
			ContentIsVisible = true;
		}
	}

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

	private bool _contentIsVisible;
	private bool _noneSelectedIsVisible;
	private bool _loadingScreenIsVisible;
	private bool _selectedContactIsVisible;
	private bool _emptyScreenIsVisible;

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
			NoneSelectedIsVisible = false;
			SelectedContactIsVisible = true;
		}
	}

	#region Visibility controls

	/// <summary>
	/// Controls wether the main content is showing or not
	/// </summary>
	public bool ContentIsVisible
	{
		get { return _contentIsVisible; }
		set { 
			_contentIsVisible = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls wether the loading screen is showing or not
	/// </summary>
	public bool LoadingScreenIsVisible
	{
		get { return _loadingScreenIsVisible; }
		set { 
			_loadingScreenIsVisible = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls wether the "empty" screen is showing or not
	/// </summary>
	public bool NoneSelectedIsVisible
	{
		get { return _noneSelectedIsVisible; }
		set {
			_noneSelectedIsVisible = value;
			NotifyOfPropertyChange();
		}
	}


	public bool EmptyScreenIsVisible
	{
		get { return _emptyScreenIsVisible; }
		set { 
			_emptyScreenIsVisible = value;
			NotifyOfPropertyChange();
		}
	}


	public bool SelectedContactIsVisible
	{
		get { return _selectedContactIsVisible; }
		set { 
			_selectedContactIsVisible = value;
			NotifyOfPropertyChange();
		}
	}


	#endregion
	#endregion
}
