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

	protected async override Task OnInitializeAsync(CancellationToken cancellationToken)
	{
		await base.OnInitializeAsync(cancellationToken);

		Items.Add(_myContacts);

		await ActivateItemAsync(_myContacts, cancellationToken);
	}
}
