using Caliburn.Micro;
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

	public async void NewCorrespondence()
	{
		await _windowManager.ShowWindowAsync(_createDraftViewModel);
	}

    public async void Inbox()
	{
		await _event.PublishOnUIThreadAsync(new NavigationEvent(NavigationEvent.NavigationRoutes.Inbox));
	}
	public async void Pipelines()
	{
		throw new NotImplementedException();
	}
	public async void Contacts()
	{
		throw new NotImplementedException();
	}
	public async void Utils()
	{
		throw new NotImplementedException();
	}
}
