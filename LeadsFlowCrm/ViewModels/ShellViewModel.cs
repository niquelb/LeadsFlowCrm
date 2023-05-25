using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class ShellViewModel : Conductor<object>
{
	private readonly InboxViewModel _inboxViewModel;

	public ShellViewModel(InboxViewModel inboxViewModel)
    {
		_inboxViewModel = inboxViewModel;

		ActivateItemAsync(_inboxViewModel);
	}
}
