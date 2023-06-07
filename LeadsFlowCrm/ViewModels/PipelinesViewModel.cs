using Caliburn.Micro;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LeadsFlowCrm.ViewModels;

public class PipelinesViewModel : Screen
{

	private readonly IContactService _contactService;
	private readonly IPipelineService _pipelineService;

	public PipelinesViewModel(
		IContactService contactService,
		IPipelineService pipelineService)
	{
		_contactService = contactService;
		_pipelineService = pipelineService;
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		Contacts = new ObservableCollection<Contact>(await _contactService.GetAllAsync());
		// TODO: Replace with real code
		Pipeline = _pipelineService.GetDemoPipeline();
	}

	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		GenerateButtons();
	}

	private void GenerateButtons()
	{
		// Clear existing buttons
		StageButtons.Clear();

		if (Pipeline.Stages == null)
		{
			return;
		}

        foreach (var stage in Pipeline.Stages)
        {
			Color color = (Color)ColorConverter.ConvertFromString(stage.Color);

			var button = new StageButton
			{
				Label = stage.Name,
				BackgroundColor = new SolidColorBrush(color),
				ClickAction = () => HandleButtonClick(stage.Id)
			};

			StageButtons.Add(button);
		}
	}

	private void HandleButtonClick(string stageId)
	{
		MessageBox.Show($"Stage {stageId} Selected");
	}


	private ObservableCollection<StageButton> _stageButtons = new();
	private ObservableCollection<Contact> _contacts = new();
	private Contact _selectedContact;
	public Pipeline Pipeline { get; set; } = new();

    public ObservableCollection<StageButton> StageButtons
	{
		get { return _stageButtons; }
		set { 
			_stageButtons = value;
			NotifyOfPropertyChange();
		}
	}

	public ObservableCollection<Contact> Contacts
	{
		get { return _contacts; }
		set { 
			_contacts = value;
			NotifyOfPropertyChange();
		}
	}

	public Contact SelectedContact
	{
		get { return _selectedContact; }
		set { 
			_selectedContact = value;
			NotifyOfPropertyChange();
		}
	}

}
