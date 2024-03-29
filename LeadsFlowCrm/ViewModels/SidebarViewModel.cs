﻿using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using System;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class SidebarViewModel
{
	private readonly IEventAggregator _event;
	private readonly IWindowManager _windowManager;
	private readonly CreateDraftViewModel _createDraftViewModel;

	public SidebarViewModel(IEventAggregator @event, IWindowManager windowManager, CreateDraftViewModel createDraftViewModel)
    {
		_event = @event;
		_windowManager = windowManager;
		_createDraftViewModel = createDraftViewModel;
	}

	public async void NewDraft()
	{
		await _windowManager.ShowWindowAsync(_createDraftViewModel);
	}

    public async void Inbox()
	{
		await _event.PublishOnUIThreadAsync(new NavigationEvent(NavigationEvent.NavigationRoutes.Inbox));
	}
	public async void Pipelines()
	{
		await _event.PublishOnUIThreadAsync(new NavigationEvent(NavigationEvent.NavigationRoutes.Pipelines));
	}
	public async void Contacts()
	{
		await _event.PublishOnUIThreadAsync(new NavigationEvent(NavigationEvent.NavigationRoutes.Contacts));
	}

	public async void Logout()
	{
		await _event.PublishOnUIThreadAsync(new LogoutEvent());
	}
}
