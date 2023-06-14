using Caliburn.Micro;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
using LeadsFlowCrm.Utils;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class CreateContactViewModel : Screen
{
	private readonly IPipelineService _pipelineService;

	public CreateContactViewModel(IPipelineService pipelineService)
    {
		_pipelineService = pipelineService;
	}

	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		LoadingSpinnerIsVisible = true;

		Pipelines = new(await LoadPipelines());

		LoadingSpinnerIsVisible = false;
		PipelinesSelectorIsVisible = true;

		if (Pipelines.Count <= 0)
		{
			Utilities.ShowNotification("No pipelines found", "You don't seem to have any pipelines!", NotificationType.Error);
		}

	}

	#region Private Methods

	/// <summary>
	/// Method for loading the list of pipelines
	/// </summary>
	/// <returns></returns>
	private async Task<IList<Pipeline>> LoadPipelines()
	{
		List<Pipeline> output = new()
		{
			await _pipelineService.GetPipelineAsync()
		};

		return output;
	}

	#endregion

	#region Public Methods

	/// <summary>
	/// Method for selecting a specific stage for the new contact
	/// </summary>
	/// <param name="stage">Selected stage</param>
	public void SelectStage(Stage stage)
	{
		SelectedStage = stage;

		StageSelectorIsVisible = false;
		StageSelectedIsVisible = true;
	}

	/// <summary>
	/// Method for selecting a specific pipeline from which select the stage
	/// </summary>
	/// <param name="pipeline">Selected pipeline</param>
	public void SelectPipeline(Pipeline pipeline)
	{
		SelectedPipeline = pipeline;

		Stages = new(SelectedPipeline.Stages.ToList());

		PipelinesSelectorIsVisible = false;
		PipelineSelectedIsVisible = true;
		StageSelectorIsVisible = true;

		if (Stages.Count <= 0)
		{
			Utilities.ShowNotification("No stages found", "This pipeline is empty!", NotificationType.Error);
		}
	}

	public async void SaveContact() { 
		//TODO check if contact exists
	}

	#endregion

	#region Properties

	/// <summary>
	/// Tab name for the view
	/// </summary>
	public string DisplayHeader { get; set; } = "Create Contact";

    #region Private property backing fields

    private bool _loadingSpinnerIsVisible;
	private bool _pipelinesSelectorIsVisible;
	private bool _stageSelectorIsVisible;
	private bool _pipelineSelectedIsVisible;
	private bool _stageSelectedIsVisible;

	private string _firstName = string.Empty;
	private string _email = string.Empty;
	private BindableCollection<Stage> _stages = new();
	private BindableCollection<Pipeline> _pipelines = new();
	private Stage _selectedStageSelectedStage = new();
	private Pipeline _selectedPipeline = new();

	#endregion

	/// <summary>
	/// List of the selected pipeline's stages
	/// </summary>
	public BindableCollection<Stage> Stages
	{
		get { return _stages; }
		set {
			_stages = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// List of the user's pipelines
	/// </summary>
	public BindableCollection<Pipeline> Pipelines
	{
		get { return _pipelines; }
		set { 
			_pipelines = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Selected pipeline
	/// </summary>
	public Pipeline SelectedPipeline
	{
		get { return _selectedPipeline; }
		set { 
			_selectedPipeline = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Selected stage
	/// </summary>
	public Stage SelectedStage
	{
		get { return _selectedStageSelectedStage; }
		set {
			_selectedStageSelectedStage = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls wether the SaveContact button is enabled
	/// </summary>
	public bool CanSaveContact
	{
		get {
			return string.IsNullOrWhiteSpace(FirstName) == false && string.IsNullOrWhiteSpace(Email) == false;
		}
		set {
			NotifyOfPropertyChange();
		}
	}


	#region Field properties

	/// <summary>
	/// First name
	/// </summary>
	public string FirstName
	{
		get { return _firstName; }
		set { 
			_firstName = value;
			NotifyOfPropertyChange();
			NotifyOfPropertyChange(nameof(CanSaveContact));
		}
	}

	/// <summary>
	/// Email
	/// </summary>
	public string Email
	{
		get { return _email; }
		set { 
			_email = value;
			NotifyOfPropertyChange();
			NotifyOfPropertyChange(nameof(CanSaveContact));
		}
	}


	#endregion

	#region Visibility controls

	/// <summary>
	/// Controls the visibility of the loading spinner
	/// </summary>
	public bool LoadingSpinnerIsVisible
	{
		get { return _loadingSpinnerIsVisible; }
		set { 
			_loadingSpinnerIsVisible = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls the visibility of the pipeline selector screen
	/// </summary>
	public bool PipelinesSelectorIsVisible
	{
		get { return _pipelinesSelectorIsVisible; }
		set {
			_pipelinesSelectorIsVisible = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls the visibility of the stage selector screen
	/// </summary>
	public bool StageSelectorIsVisible
	{
		get { return _stageSelectorIsVisible; }
		set {
			_stageSelectorIsVisible = value; 
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls the visibility of the selected pipeline screen
	/// </summary>
	public bool PipelineSelectedIsVisible
	{
		get { return _pipelineSelectedIsVisible; }
		set { 
			_pipelineSelectedIsVisible = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls the visibility of the selected stage
	/// </summary>
	public bool StageSelectedIsVisible
	{
		get { return _stageSelectedIsVisible; }
		set {
			_stageSelectedIsVisible = value;
			NotifyOfPropertyChange();
		}
	}

	#endregion

	#endregion
}
