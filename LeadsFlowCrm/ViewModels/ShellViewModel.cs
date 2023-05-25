using Caliburn.Micro;
using LeadsFlowCrm.EventModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class ShellViewModel : Conductor<object>, IHandle<EmailSelectedEvent>
{
	private readonly InboxViewModel _inboxViewModel;
	private readonly IEventAggregator _event;

	public ShellViewModel(InboxViewModel inboxViewModel, IEventAggregator @event)
    {
		_inboxViewModel = inboxViewModel;
		_event = @event;
		_event.SubscribeOnUIThread(this);

		ActivateItemAsync(_inboxViewModel);
	}

	public async Task HandleAsync(EmailSelectedEvent e, CancellationToken cancellationToken)
	{
	}
}
