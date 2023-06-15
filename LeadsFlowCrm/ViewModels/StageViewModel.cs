using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
using LeadsFlowCrm.Utils;
using Notifications.Wpf;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class StageViewModel : Screen, IHandle<StageSelectedEvent>
{
	private readonly IContactService _contactService;

	public StageViewModel(IContactService contactService,
					   IEventAggregator @event)
    {
		_contactService = contactService;
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

	/// <summary>
	/// Selected contact
	/// </summary>
	public Contact? SelectedContact { get; set; }

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
