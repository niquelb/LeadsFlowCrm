using Caliburn.Micro;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class ContactsShellViewModel : Conductor<object>.Collection.OneActive
{
	private readonly MyContactsViewModel _myContacts;

	public ContactsShellViewModel(MyContactsViewModel myContacts)
    {
		_myContacts = myContacts;
	}

	protected async override Task OnActivateAsync(CancellationToken cancellationToken)
	{
		await base.OnActivateAsync(cancellationToken);

		/*
		 * This is to fix a weird issue with the tabcontroller binding to the Items collection.
		 * If not done the view doesn't load properly again once navigated away from
		 */
        if (Items.Count <= 0)
        {
			Items.Add(_myContacts);
        }
	}

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		Items.Clear();
		Items.Add(_myContacts);
	}
}
