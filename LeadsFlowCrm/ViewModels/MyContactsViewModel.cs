using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class MyContactsViewModel : Screen
{
    public MyContactsViewModel(IContactService contactService)
    {
		_contactService = contactService;
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

	}

	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		ShowLoadingScreen = true;

		// We retrieve the contacts from the API
		Contacts = new BindableCollection<Contact>(await GetContactsAsync());

		ShowLoadingScreen = false;

		// If none contacts found, we show the "empty" screen
		if (Contacts.Count <= 0)
		{
			ShowEmptyScreen = true;
		}
		else
		{
			ShowContent = true;
		}
	}

	/// <summary>
	/// Method for retrieving the contacts from the API
	/// </summary>
	/// <returns>List of Contacts</returns>
	private async Task<IList<Contact>> GetContactsAsync()
	{
		return await _contactService.GetAllAsync();
	}

	public string DisplayHeader { get; set; } = "My Contacts";

    /*
	 * Private backing fields
	 */
    private bool _showContent;
	private bool _showEmptyScreen;
	private bool _showLoadingScreen;
	private BindableCollection<Contact> _contacts = new();
	private Contact _selectedContact = new();
	private readonly IContactService _contactService;

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
}
