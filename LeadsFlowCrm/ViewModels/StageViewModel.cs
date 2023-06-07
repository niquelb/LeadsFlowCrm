using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		Contacts = new ObservableCollection<Contact>(await _contactService.GetAllAsync());
	}

	/// <summary>
	/// Event that gets triggered when the user switches between stages in the Pipelines screen
	/// </summary>
	/// <param name="e">StageSelectedEvent object which holds the selected stage</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task HandleAsync(StageSelectedEvent e, CancellationToken cancellationToken)
	{
		Stage = e.SelectedStage;
	}

	/// <summary>
	/// Stage
	/// </summary>
	public Stage Stage { get; set; } = new();

	/*
	 * Private backing fields for the properties
	 */
	private ObservableCollection<Contact> _contacts = new();

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


}
