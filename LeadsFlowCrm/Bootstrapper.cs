using Caliburn.Micro;
using LeadsFlowCrm.Models;
using LeadsFlowCrm.Services;
using LeadsFlowCrm.Services.ModelServices;
using LeadsFlowCrm.Utils;
using LeadsFlowCrm.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace LeadsFlowCrm;

/// <summary>
/// Class responsible for setting up all the necessary backend for the app and configuring CaliburnMicro
/// </summary>
public class Bootstrapper : BootstrapperBase
{
	// SimpleContainer is used for Dependency Injection
	private SimpleContainer _container = new();
    public Bootstrapper()
    {
        Initialize();
    }

	protected override void Configure()
	{
		// This instance will be returned whenever we "ask" for our Container
		_container.Instance(_container);

		_container
			.Singleton<IWindowManager, WindowManager>()
			.Singleton<IEventAggregator, EventAggregator>()
			.Singleton<IApiHelper, ApiHelper>()
			.PerRequest<IUserService,  UserService>()
			.PerRequest<IContactService, ContactService>()
			.PerRequest<IPipelineService, PipelineService>()
			.PerRequest<IStageService, StageService>()
			.Singleton<LoggedInUser>() //TODO: implement an interface
			.Singleton<IBaseGoogleServiceClass, BaseGoogleServiceClass>()
			.Singleton<IOAuthServiceClass, OAuthServiceClass>()
			.Singleton<IPeopleServiceClass, PeopleServiceClass>()
			.Singleton<IGmailServiceClass, GmailServiceClass>();

		// Adds all of our ViewModels into Dependency Injection
		GetType().Assembly.GetTypes()
			.Where(t => t.IsClass)
			.Where(t => t.Name.EndsWith("ViewModel"))
			.ToList()
			.ForEach(t => _container.RegisterPerRequest(t, t.ToString(), t));
	}

	protected override void OnStartup(object sender, StartupEventArgs e)
	{
		// Set "ShellView" as the startup window
		DisplayRootViewForAsync<LoginViewModel>();
	}

	protected override object GetInstance(Type service, string key)
	{
		// Get the instance of a specified type using a key
		return _container.GetInstance(service, key);
	}

	protected override IEnumerable<object> GetAllInstances(Type service)
	{
		// Get all instances of a specified type
		return _container.GetAllInstances(service);
	}

	protected override void BuildUp(object instance)
	{
		_container.BuildUp(instance);
	}
}
