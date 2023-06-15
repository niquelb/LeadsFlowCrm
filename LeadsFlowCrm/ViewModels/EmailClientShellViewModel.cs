using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class EmailClientShellViewModel : Conductor<object>.Collection.OneActive
{
	private readonly InboxViewModel _inbox;
	private readonly DraftsViewModel _drafts;
	private readonly AllMailViewModel _allMail;

	public EmailClientShellViewModel(InboxViewModel inbox,
								  DraftsViewModel drafts,
								  AllMailViewModel allMail)
    {
		_inbox = inbox;
		_drafts = drafts;
		_allMail = allMail;
	}
	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		/*
		 * This is to fix a weird issue with the tabcontroller binding to the Items collection.
		 * If not done the view doesn't load properly again once navigated away from
		 */
		if (Items.Any())
		{
			AddItems();
		}
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		AddItems();
	}

	/// <summary>
	/// Method for adding the screens to the Items collection, this is a separate method because due to the binding of the tab controller it needs to be called twice
	/// </summary>
	private void AddItems()
	{
		Items.Clear();
		Items.Add(_inbox);
		Items.Add(_allMail);
		Items.Add(_drafts);
	}
}
