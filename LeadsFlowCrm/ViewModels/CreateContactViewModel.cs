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
using System.Windows;

namespace LeadsFlowCrm.ViewModels;

public class CreateContactViewModel : Screen
{
	private readonly IPipelineService _pipelineService;
	private readonly IContactService _contactService;
	private readonly LoggedInUser _loggedInUser;

	public CreateContactViewModel(IPipelineService pipelineService,
							   IContactService contactService,
							   LoggedInUser loggedInUser)
    {
		_pipelineService = pipelineService;
		_contactService = contactService;
		_loggedInUser = loggedInUser;
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

	/// <summary>
	/// Method used to dispose and clear all the fields and selected pipelines/stages
	/// </summary>
	private void ClearFields()
	{
		// Clear the fields
		FirstName = string.Empty;
		LastNames = string.Empty;
		Email = string.Empty;
		Phone = string.Empty;
		WebSite = string.Empty;
		Company = string.Empty;
		JobTitle = string.Empty;
		Location = string.Empty;
		Address = string.Empty;

		// Clear the stages and pipeline
		SelectedStage = null;
		SelectedPipeline = null;
		Stages = new();

		// Modify visibility
		StageSelectedIsVisible = false;
		StageSelectorIsVisible = false;
		PipelineSelectedIsVisible = false;
		PipelinesSelectorIsVisible = true;

	}

	/// <summary>
	/// Method for checking if the contact exists based on a given email address
	/// </summary>
	/// <param name="email">Email address</param>
	/// <returns>True if the contact exists, false if not</returns>
	/// <exception cref="ArgumentNullException">If the email is null/empty</exception>
	private async Task<bool> CheckIfExists(string email)
	{
		if (string.IsNullOrWhiteSpace(email))
		{
			throw new ArgumentNullException(nameof(email));
		}

		Contact? contact = await _contactService.GetByEmailAsync(email);

		return contact != null;
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

	/// <summary>
	/// Method for saving the newly created contact
	/// </summary>
	public async void SaveContact() {
        if (await CheckIfExists(Email))
		{
			Utilities.ShowNotification("Contact already exists", $"A contact with this email address already exists. ({Email})", NotificationType.Error);
			return;
		}

        if (SelectedStage == null)
		{
			// TODO prettify this message box
			MessageBoxResult messageBoxResult = MessageBox.Show("There are no stages selected for this contact, do you wish to create it anyway? You can assign a stage to it later in the 'My Contacts' screen.", 
				"No stage selected", MessageBoxButton.YesNo);
			if (messageBoxResult == MessageBoxResult.No)
			{
				Utilities.ShowNotification("Contact creation cancelled", "The contact was not created", NotificationType.Information);
				return;
			}

			Utilities.ShowNotification("No stage assigned", "The contact will be created without any stage assigned to it", NotificationType.Warning);
		}

		CreatedContact = new()
		{
			Email = Email,
			FirstName = FirstName,
			LastNames = LastNames,
			Phone = Phone,
			Notes = Notes,
		};

        try
		{
			await _contactService.PostToApiAsync(CreatedContact, _loggedInUser.Id, SelectedStage?.Id);
			Utilities.ShowNotification("Creation successfull", "The contact was created successfully", NotificationType.Success);
			ClearFields();
		}
		catch (Exception ex)
		{
			Utilities.ShowNotification("Creation failed", $"There was an error creating the contact ({ex.Message})", NotificationType.Error);
		}
    }

	/// <summary>
	/// Method for discarding the contact, clears all the fields and selected pipeline/stage
	/// </summary>
	public void DiscardContact()
	{
		ClearFields();
	}

	/// <summary>
	/// Method for de-selecting the pipeline (and consequently the stage if there is one)
	/// </summary>
	public void DiscardPipeline()
	{
		SelectedStage = null;
		SelectedPipeline = null;
		Stages = new();

		StageSelectedIsVisible = false;
		StageSelectorIsVisible = false;
		PipelineSelectedIsVisible = false;
		PipelinesSelectorIsVisible = true;
	}
		
	/// <summary>
	/// Method for de-selecting the stage
	/// </summary>
	public void DiscardStage()
	{
		SelectedStage = null;

		StageSelectorIsVisible = true;
		StageSelectedIsVisible = false;
	}

	#endregion

	#region Properties

	/// <summary>
	/// Tab name for the view
	/// </summary>
	public string DisplayHeader { get; } = "Create Contact";

	/// <summary>
	/// Contact that will be created
	/// </summary>
	public Contact CreatedContact { get; set; } = new();

    #region Private property backing fields

    private bool _loadingSpinnerIsVisible;
	private bool _pipelinesSelectorIsVisible;
	private bool _stageSelectorIsVisible;
	private bool _pipelineSelectedIsVisible;
	private bool _stageSelectedIsVisible;

	private string _firstName = string.Empty;
	private string _lastNames = string.Empty;
	private string _email = string.Empty;
	private string _phone = string.Empty;
	private string _website = string.Empty;
	private string _company = string.Empty;
	private string _jobTitle = string.Empty;
	private string _location = string.Empty;
	private string _address = string.Empty;
	private string _notes;

	private BindableCollection<Stage> _stages = new();
	private BindableCollection<Pipeline> _pipelines = new();
	private Stage _selectedStage;
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
	public Pipeline? SelectedPipeline
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
	public Stage? SelectedStage
	{
		get { return _selectedStage; }
		set {
			_selectedStage = value;
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
	/// Last names
	/// </summary>
	public string LastNames
	{
		get { return _lastNames; }
		set { 
			_lastNames = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Email
	/// </summary>
	public string Email
	{
		get { return _email; }
		set
		{
			_email = value;
			NotifyOfPropertyChange();
			NotifyOfPropertyChange(nameof(CanSaveContact));
		}
	}

	/// <summary>
	/// Phone number
	/// </summary>
	public string Phone
	{
		get { return _phone; }
		set { 
			_phone = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Website
	/// </summary>
	public string WebSite
	{
		get { return _website; }
		set { 
			_website = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Compan name
	/// </summary>
	public string Company
	{
		get { return _company; }
		set { 
			_company = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Job title
	/// </summary>
	public string JobTitle
	{
		get { return _jobTitle; }
		set {
			_jobTitle = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Location
	/// </summary>
	public string Location
	{
		get { return _location; }
		set {
			_location = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Address
	/// </summary>
	public string Address
	{
		get { return _address; }
		set {
			_address = value;
			NotifyOfPropertyChange();
		}
	}

	/// <summary>
	/// Notes
	/// </summary>
	public string Notes
	{
		get { return _notes; }
		set { 
			_notes = value;
			NotifyOfPropertyChange();
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
