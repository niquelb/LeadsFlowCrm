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

public class ShellViewModel : Conductor<object>, IHandle<EmailSelectedEvent>, IHandle<NavigationEvent>
{
	private readonly InboxViewModel _inboxViewModel;
	private readonly PipelinesViewModel _pipelinesViewModel;
	private readonly SelectedEmailViewModel _selectedEmailViewModel;
	private readonly ContactsShellViewModel _contactsShellViewModel;
	private readonly IEventAggregator _event;
	private readonly IUserService _userService;

	public ShellViewModel(InboxViewModel inboxViewModel,
					   PipelinesViewModel pipelinesViewModel,
					   SelectedEmailViewModel selectedEmailViewModel,
					   ContactsShellViewModel contactsShellViewModel,
					   SidebarViewModel sidebarViewModel,
					   IEventAggregator @event,
					   IUserService userService)
    {
		_inboxViewModel = inboxViewModel;
		_pipelinesViewModel = pipelinesViewModel;
		_selectedEmailViewModel = selectedEmailViewModel;
		_contactsShellViewModel = contactsShellViewModel;
		Sidebar = sidebarViewModel;
		_event = @event;
		_userService = userService;
		_event.SubscribeOnUIThread(this);

		ActivateItemAsync(_inboxViewModel);
	}

	/// <summary>
	/// Logout method
	/// </summary>
	public async void Logout()
	{
		await _userService.LogoutUserAsync();

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
				await ActivateItemAsync(_inboxViewModel, cancellationToken);
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
}
