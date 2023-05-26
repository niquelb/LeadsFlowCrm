using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using System;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class SidebarViewModel : Screen
{
	private readonly IEventAggregator _event;

	public SidebarViewModel(IEventAggregator @event)
    {
		_event = @event;
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
