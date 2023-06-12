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
		Contacts = new ObservableCollection<Contact>(await _contactService.GetByStageAsync(Stage.Id));

		// Show the "empty" screen if no contacts found
		IsStageEmpty = Contacts.Count <= 0;
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
	public Contact SelectedContact { get; set; } = new();

	#region Property backing fields
	private bool _isStageEmpty;
	private ObservableCollection<Contact> _contacts = new();
	#endregion

	/// <summary>
	/// Contacts for the stage
	/// </summary>
	public ObservableCollection<Contact> Contacts
	{
		get { return _contacts; }
		set { 
			_contacts = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Wether the stage has contacts or is empty
	/// </summary>
	public bool IsStageEmpty
	{
		get { return _isStageEmpty; }
		set { 
			_isStageEmpty = value;
			NotifyOfPropertyChange();
		}
	}
	#endregion

}
