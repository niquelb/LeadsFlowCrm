using Caliburn.Micro;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
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

		//TODO: if only one pipeline exists skip the user selecting the pipeline step

		LoadingSpinnerIsVisible = false;
		PipelinesSelectorIsVisible = true;

	}

	#region Private Methods

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

	public void SelectStage(Stage stage)
	{
		Trace.WriteLine(stage.Name, "SELECTED STAGE");
	}

	public void SelectPipeline(Pipeline pipeline)
	{
		SelectedPipeline = pipeline;

		Stages = new(SelectedPipeline.Stages.ToList());

		PipelinesSelectorIsVisible = false;
		StageSelectorIsVisible = true;
	}

	#endregion

	#region Properties

	/// <summary>
	/// Tab name for the view
	/// </summary>
	public string DisplayHeader { get; set; } = "Create Contact";

    #region Private property backing fields
    private bool _loadingSpinnerIsVisible;
	private bool _emptyScreenIsVisible;
	private bool _pipelinesSelectorIsVisible;
	private bool _stageSelectorIsVisible;
	private string _emptyScreenText = "No pipelines found!";
	private BindableCollection<Stage> _stages = new();
	private BindableCollection<Pipeline> _pipelines = new();
	private Stage _selectedStageSelectedStage;
	private Pipeline _selectedPipeline;
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
	public Stage MyProperty
	{
		get { return _selectedStageSelectedStage; }
		set {
			_selectedStageSelectedStage = value;
			NotifyOfPropertyChange();
		}
	}


	/// <summary>
	/// Text that will be displayed in the "empty" message/screen
	/// </summary>
	public string EmptyScreenText
	{
		get { return _emptyScreenText; }
		set { 
			_emptyScreenText = value;
			NotifyOfPropertyChange();
		}
	}


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
	/// Controls the visibility of the "empty" message
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


	#endregion

	#endregion
}
