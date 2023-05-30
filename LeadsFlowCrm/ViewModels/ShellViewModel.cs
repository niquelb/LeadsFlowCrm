﻿using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class ShellViewModel : Conductor<object>, IHandle<EmailSelectedEvent>, IHandle<NavigationEvent>
{
	private readonly InboxViewModel _inboxViewModel;
	private readonly PipelinesViewModel _pipelinesViewModel;
	private readonly SelectedEmailViewModel _selectedEmailViewModel;
	private readonly IEventAggregator _event;

	public ShellViewModel(InboxViewModel inboxViewModel,
					   PipelinesViewModel pipelinesViewModel,
					   SelectedEmailViewModel selectedEmailViewModel,
					   SidebarViewModel sidebarViewModel,
					   IEventAggregator @event)
    {
		_inboxViewModel = inboxViewModel;
		_pipelinesViewModel = pipelinesViewModel;
		_selectedEmailViewModel = selectedEmailViewModel;
		Sidebar = sidebarViewModel;
		_event = @event;
		_event.SubscribeOnUIThread(this);

		ActivateItemAsync(_inboxViewModel);
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
			default:
				throw new ArgumentException($"{nameof(e.Route)} is not a valid navigation object or is not yet implemented");
		}
	}
}
