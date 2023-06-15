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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class MyContactsViewModel : Screen
{
	private readonly IContactService _contactService;
	private readonly IEventAggregator _event;
	private readonly IWindowManager _windowManager;
	private readonly IPipelineService _pipelineService;
	private readonly IStageService _stageService;
	private readonly CreateDraftViewModel _createDraft;
	private readonly LoggedInUser _loggedInUser;

	public MyContactsViewModel(IContactService contactService,
							IEventAggregator @event,
							IWindowManager windowManager,
							IPipelineService pipelineService,
							IStageService stageService,
							CreateDraftViewModel createDraft,
							LoggedInUser loggedInUser)
    {
		_contactService = contactService;
		_event = @event;
		_windowManager = windowManager;
		_pipelineService = pipelineService;
		_stageService = stageService;
		_createDraft = createDraft;
		_loggedInUser = loggedInUser;
	}

	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		SelectedContact = null;
		await LoadContacts();

		AssignedPipeline = await _pipelineService.GetPipelineAsync();
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

	/// <summary>
	/// Method to create a new draft with the selected contact as the recipient
	/// </summary>
	public async void NewDraft()
	{
		await _windowManager.ShowWindowAsync(_createDraft);

		await _event.PublishOnUIThreadAsync(new DraftEvent() { 
			Recipients = new List<string>() { 
				SelectedContact.Email
			} 
		});
	}
	
	/// <summary>
	/// Method to update the assigned stage for the user
	/// </summary>
	/// <param name="cStage">ContactStage object</param>
	public async void SelectStage(ContactStage cStage)
	{
        if (SelectedContact == null)
        {
			return;
        }

        if (string.IsNullOrWhiteSpace(cStage.GivenStage.Id))
        {
			throw new ArgumentNullException(nameof(cStage.GivenStage.Id));
        }

		Utilities.ShowNotification("Updating stage", "Updating the contact's stage...", NotificationType.Information);

		// We update the contact in the API
        await _contactService.ModifyStageIdToApiAsync(SelectedContact.Id, cStage.GivenStage.Id);

		Utilities.ShowNotification("Success", "Successfully updated the contact's stage", NotificationType.Success);

		// We refresh the stages
		await LoadContacts();
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
		if (Contacts.Any())
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

	/// <summary>
	/// Method for loading the selected assigned pipeline's stages
	/// </summary>
	/// <returns></returns>
	private async Task LoadContactStages()
	{
        if (SelectedContact == null)
        {
			return;
        }

        Stages.Clear();

		var apiStages = await _stageService.GetByPipelineAsync(AssignedPipeline.Id);

        foreach (var s in apiStages)
        {
			bool isCurrent = SelectedContact.StageId != null && SelectedContact.StageId == s.Id;

			Stages.Add(new()
			{
				GivenStage = s,
				StageName = s.Name,
				IsCurrentStage = isCurrent,
			});
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
	private Contact? _selectedContact = new();
	private BindableCollection<ContactStage> _stages = new();
	private Pipeline _assignedPipeline = new();

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
	public Contact? SelectedContact
	{
		get { return _selectedContact; }
		set { 
			_selectedContact = value;
			NotifyOfPropertyChange();

            if (value != null)
            {
				// We display the contact info
				NoneSelectedIsVisible = false;
				SelectedContactIsVisible = true;

				LoadContactStages();
			}
        }
	}

	/// <summary>
	/// Assigned pipeline
	/// </summary>
	public Pipeline AssignedPipeline
	{
		get { return _assignedPipeline; }
		set { 
			_assignedPipeline = value;
			NotifyOfPropertyChange();
		}
	}


	/// <summary>
	/// Stages from the pipeline
	/// </summary>
	public BindableCollection<ContactStage> Stages
	{
		get { return _stages; }
		set { 
			_stages = value; 
			NotifyOfPropertyChange();
		}
	}


	#region Visibility controls

	/// <summary>
	/// Visibility control for the main content
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
	/// Visibility control for the loading screen
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
	/// Visibility control for the "no contact selected" view
	/// </summary>
	public bool NoneSelectedIsVisible
	{
		get { return _noneSelectedIsVisible; }
		set {
			_noneSelectedIsVisible = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Visibility control for the "empty" screen
	/// </summary>
	public bool EmptyScreenIsVisible
	{
		get { return _emptyScreenIsVisible; }
		set { 
			_emptyScreenIsVisible = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Visibility control for the contact details view
	/// </summary>
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
