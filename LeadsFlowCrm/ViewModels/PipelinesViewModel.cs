using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services.ModelServices;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace LeadsFlowCrm.ViewModels;

public class PipelinesViewModel : Conductor<object>
{

	private readonly IPipelineService _pipelineService;
	private readonly IEventAggregator _event;

	public PipelinesViewModel(IPipelineService pipelineService,
						   StageViewModel stageViewModel,
						   IEventAggregator @event)
	{
		_pipelineService = pipelineService;
		_event = @event;
		ActivateItemAsync(stageViewModel);
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		// We retrieve the pipeline from the API
		Pipeline = await _pipelineService.GetPipelineAsync();

		// We fill in the stage tabs
		FillStages();

		IsLoading = false;
	}

	/// <summary>
	/// Method for filling in the stage tabs for the pipeline
	/// </summary>
	private void FillStages()
	{
		/*
		* We iterate over all the stages in the Pipeline and create tabs to populate the TabControl
		*/
		foreach (var stage in Pipeline.Stages)
		{
			// We make the color based on the hex value from the model
			Color color = (Color)ColorConverter.ConvertFromString(stage.Color);

			var tab = new TabItem()
			{
				Header = stage.Name,
				MinWidth = 200,
				HorizontalAlignment = HorizontalAlignment.Center,
				Background = new SolidColorBrush(color),
			};

			// This is what displays the Stage screen inside each tab
			tab.SetBinding(ContentControl.ContentProperty, new Binding("ActiveItem"));

			// We add the tab to the list
			Tabs.Add(tab);
		}
	}

	/// <summary>
	/// Pipeline for the view
	/// </summary>
	public Pipeline Pipeline { get; set; } = new();

    /*
	 * Private backing fields for the properties
	 */
    private ObservableCollection<TabItem> _tabs = new();
	private TabItem _selectedTab = new();
	private bool _isLoading = true;

	/// <summary>
	/// Controls wether the view is displaying the loading indicator
	/// </summary>
	public bool IsLoading
	{
		get { return _isLoading; }
		set { 
			_isLoading = value;
			NotifyOfPropertyChange();
		}
	}


	/// <summary>
	/// Tabs that represent each stage and it's content
	/// </summary>
	public ObservableCollection<TabItem> Tabs
	{
		get { return _tabs; }
		set { 
			_tabs = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Selected tab (stage)
	/// </summary>
	public TabItem SelectedTab
	{
		get { return _selectedTab; }
		set { 
			_selectedTab = value;
			NotifyOfPropertyChange();

			if (_selectedTab != null)
			{
				int selectedIndex = Tabs.IndexOf(_selectedTab);
				if (selectedIndex >= 0 && selectedIndex < Pipeline.Stages.Count)
				{
					var selectedStage = Pipeline.Stages[selectedIndex];

					_event.PublishOnUIThreadAsync(new StageSelectedEvent()
					{
						SelectedStage = selectedStage
					});
				}
			}
		}
	}
}
