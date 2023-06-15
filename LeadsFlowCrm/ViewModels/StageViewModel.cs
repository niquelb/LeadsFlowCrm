using Caliburn.Micro;
using Google.Apis.PeopleService.v1.Data;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
using LeadsFlowCrm.Utils;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class StageViewModel : Screen, IHandle<StageSelectedEvent>
{
	private readonly IContactService _contactService;
	private readonly IEventAggregator _event;
	private readonly IWindowManager _windowManager;
	private readonly CreateDraftViewModel _createDraft;

	public StageViewModel(IContactService contactService,
					   IEventAggregator @event,
					   IWindowManager windowManager,
					   CreateDraftViewModel createDraft)
    {
		_contactService = contactService;
		_event = @event;
		_windowManager = windowManager;
		_createDraft = createDraft;
		@event.SubscribeOnUIThread(this);
	}

	#region Events
	/// <summary>
	/// Event that gets triggered when the user switches between stages in the Pipelines screen
	/// </summary>
	/// <param name="e">StageSelectedEvent object which holds the selected stage</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task HandleAsync(StageSelectedEvent e, CancellationToken cancellationToken)
	{
		Stage = e.SelectedStage;

		await LoadContacts();

    }
	#endregion

	#region Public Methods

	/// <summary>
	/// Method to refresh the contacts list
	/// </summary>
	/// <returns></returns>
	public async Task RefreshStage()
	{
		Utilities.ShowNotification("Reloading stage", "Reloading the current stage...", NotificationType.Information);
		await LoadContacts();
	}

	/// <summary>
	/// Method for creating a new draft with the selcted contact's email as the recipient
	/// </summary>
	/// <param name="contact">Selected contact</param>
	public async void NewDraft(Contact contact)
	{
		await _windowManager.ShowWindowAsync(_createDraft);

		await _event.PublishOnUIThreadAsync(new DraftEvent()
		{
			Recipients = new List<string>() {
				contact.Email
			}
		});
	}

	/// <summary>
	/// Method from removing the selected contact from the stage
	/// </summary>
	/// <param name="contact">Selected contact</param>
	public async void Remove(Contact contact)
	{
		contact.StageId = string.Empty;

		Utilities.ShowNotification("Removing contact from stage", "Removing the selected contact from the stage...", NotificationType.Information);
		try
		{
			await _contactService.ModifyStageIdToApiAsync(contact.Id, "887b03e7-3037-4e14-a906-ca0424aee547"); // TODO DO NOT DO THIS, this is a temporary fix but it is manually moving the contact to a stage in another pipeline
			Contacts.Remove(contact);
			Utilities.ShowNotification("Removed contact from stage", "Successfully removed the contact from the stage.", NotificationType.Success);
		}
		catch (Exception ex)
		{
			Utilities.ShowNotification("Failed removing process", $"Failed to remove the contact from the stage. ({ex.Message})", NotificationType.Error);
		}
	}

	#endregion

	#region Private Methods

	/// <summary>
	/// Method that loads the Contacts collection from the API and shows/hides the "empty" screen
	/// </summary>
	/// <returns></returns>
	private async Task LoadContacts()
	{
		EmptyScreenIsVisible = false;
		ContentIsVisible = false;

		Contacts = new BindableCollection<Contact>(await _contactService.GetByStageAsync(Stage.Id));

		if (Contacts.Any())
		{
			ContentIsVisible = true;
		}
        else
		{
			EmptyScreenIsVisible = true;
        }
    }

	#endregion

	#region Properties
	/// <summary>
	/// Stage
	/// </summary>
	public Stage Stage { get; set; } = new();

	#region Property backing fields

	private bool _emptyScreenIsVisible;
	private bool _contentIsVisible;

	private BindableCollection<Contact> _contacts = new();
	#endregion

	/// <summary>
	/// Contacts for the stage
	/// </summary>
	public BindableCollection<Contact> Contacts
	{
		get { return _contacts; }
		set { 
			_contacts = value;
			NotifyOfPropertyChange();
		}
	}

	#region Visibility controls

	/// <summary>
	/// Controls the visibility of the "empty" screen
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
	/// Controls the visibility of the content
	/// </summary>
	public bool ContentIsVisible
	{
		get { return _contentIsVisible; }
		set {
			_contentIsVisible = value;
			NotifyOfPropertyChange();
		}
	}
	#endregion

	#endregion

}
