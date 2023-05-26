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
	private readonly SelectedEmailViewModel _selectedEmailViewModel;
	private readonly IEventAggregator _event;

	public ShellViewModel(InboxViewModel inboxViewModel,
					   SelectedEmailViewModel selectedEmailViewModel,
					   IEventAggregator @event)
    {
		_inboxViewModel = inboxViewModel;
		_selectedEmailViewModel = selectedEmailViewModel;
		_event = @event;
		_event.SubscribeOnUIThread(this);

		ActivateItemAsync(_inboxViewModel);
	}

	/// <summary>
	/// Method that gets executed when the user clicks (or opens) an email from their inbox
	/// </summary>
	/// <param name="e">Event model</param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public async Task HandleAsync(EmailSelectedEvent e, CancellationToken cancellationToken)
	{
		await ActivateItemAsync(_selectedEmailViewModel);
	}
}
