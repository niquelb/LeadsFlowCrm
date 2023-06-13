using Caliburn.Micro;
using LeadsFlowCrm.Models;
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
    public CreateContactViewModel()
    {
			
    }

	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		Stages.Add(new()
		{
			Name = "Test stage"
		});
		Stages.Add(new()
		{
			Name = "Test stage 2"
		});

		ShowLoadingScreen = false;
		ShowContent = true;

	}

	#region Public Methods

	public void SelectStage(Stage stage)
	{
		Trace.WriteLine(stage.Name, "SELECTED STAGE");
	}

	#endregion

	#region Properties

	/// <summary>
	/// Tab name for the view
	/// </summary>
	public string DisplayHeader { get; set; } = "Create Contact";

	#region Private property backing fields
	private bool _showLoadingScreen;
	private bool _showContent;
	private BindableCollection<Stage> _stages = new();
	#endregion

	/// <summary>
	/// List of the user's available stages
	/// </summary>
	public BindableCollection<Stage> Stages
	{
		get { return _stages; }
		set {
			_stages = value;
			NotifyOfPropertyChange();
		}
	}

	#region Visibility controls
	/// <summary>
	/// Controls wether or not the loading screen is visible
	/// </summary>
	public bool ShowLoadingScreen
	{
		get { return _showLoadingScreen; }
		set { 
			_showLoadingScreen = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Controls wether or not the main content is visible
	/// </summary>
	public bool ShowContent
	{
		get { return _showContent; }
		set { 
			_showContent = value;
			NotifyOfPropertyChange();
		}
	}
	#endregion

	#endregion
}
