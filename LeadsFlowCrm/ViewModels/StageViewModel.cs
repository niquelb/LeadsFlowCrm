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

	public async Task HandleAsync(StageSelectedEvent e, CancellationToken cancellationToken)
	{
		Stage = e.SelectedStage;

		Trace.WriteLine(Stage.Name, "Stage -> ");
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		Contacts = new ObservableCollection<Contact>(await _contactService.GetAllAsync());
	}

	public Stage Stage { get; set; } = new();

	private ObservableCollection<Contact> _contacts = new();

	public ObservableCollection<Contact> Contacts
	{
		get { return _contacts; }
		set { 
			_contacts = value;
			NotifyOfPropertyChange();
		}
	}


}
