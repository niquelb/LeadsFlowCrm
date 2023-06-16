using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using LeadsFlowCrm.Services.ModelServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace LeadsFlowCrm.ViewModels;

public class ShellViewModel : Conductor<object>, IHandle<EmailSelectedEvent>, IHandle<NavigationEvent>, IHandle<LogoutEvent>
{
	private readonly EmailClientShellViewModel _emailClientShellViewModel;
	private readonly PipelinesViewModel _pipelinesViewModel;
	private readonly SelectedEmailViewModel _selectedEmailViewModel;
	private readonly ContactsShellViewModel _contactsShellViewModel;
	private readonly CreateDraftViewModel _createDraftViewModel;
	private readonly IEventAggregator _event;
	private readonly IWindowManager _windowManager;
	private readonly IUserService _userService;

	public ShellViewModel(EmailClientShellViewModel emailClientShellViewModel,
					   PipelinesViewModel pipelinesViewModel,
					   SelectedEmailViewModel selectedEmailViewModel,
					   ContactsShellViewModel contactsShellViewModel,
					   SidebarViewModel sidebarViewModel,
					   CreateDraftViewModel createDraftViewModel,
					   IEventAggregator @event,
					   IWindowManager windowManager,
					   IUserService userService)
    {
		_emailClientShellViewModel = emailClientShellViewModel;
		_pipelinesViewModel = pipelinesViewModel;
		_selectedEmailViewModel = selectedEmailViewModel;
		_contactsShellViewModel = contactsShellViewModel;
		Sidebar = sidebarViewModel;
		_createDraftViewModel = createDraftViewModel;
		_event = @event;
		_windowManager = windowManager;
		_userService = userService;
		_event.SubscribeOnUIThread(this);

		ActivateItemAsync(_emailClientShellViewModel);
	}

	/// <summary>
	/// Logout method
	/// </summary>
	public async void Logout()
	{
		await _userService.LogoutUserAsync();

		await TryCloseAsync();
	}

	public async void NewDraft()
	{
		await _windowManager.ShowWindowAsync(_createDraftViewModel);
	}

	public async void Exit()
	{
		await TryCloseAsync();
	}

	public SidebarViewModel Sidebar { get; }

	/// <summary>
	/// Event that gets triggered when the user clicks (or opens) an email from their inbox
	/// </summary>
	/// <param name="e">Event model</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task HandleAsync(EmailSelectedEvent e, CancellationToken cancellationToken)
	{
		await ActivateItemAsync(_selectedEmailViewModel);
	}

	/// <summary>
	/// Event for navigation in the application
	/// </summary>
	/// <param name="e">Event model</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task HandleAsync(NavigationEvent e, CancellationToken cancellationToken)
	{
		switch (e.Route)
		{
			case NavigationEvent.NavigationRoutes.Inbox:
				await ActivateItemAsync(_emailClientShellViewModel, cancellationToken);
				break;
			case NavigationEvent.NavigationRoutes.Pipelines:
				await ActivateItemAsync(_pipelinesViewModel, cancellationToken);
				break;
			case NavigationEvent.NavigationRoutes.Contacts:
				await ActivateItemAsync(_contactsShellViewModel, cancellationToken);
				break;
			default:
				throw new ArgumentException($"{nameof(e.Route)} is not a valid navigation object or is not yet implemented");
		}
	}

	public async Task HandleAsync(LogoutEvent message, CancellationToken cancellationToken)
	{
		Logout();
	}
}
