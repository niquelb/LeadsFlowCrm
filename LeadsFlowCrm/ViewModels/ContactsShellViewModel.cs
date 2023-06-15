using Caliburn.Micro;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LeadsFlowCrm.ViewModels;

public class ContactsShellViewModel : Conductor<object>.Collection.OneActive
{
	private readonly MyContactsViewModel _myContacts;
	private readonly ImportContactsViewModel _importContacts;
	private readonly CreateContactViewModel _createContact;

	public ContactsShellViewModel(MyContactsViewModel myContacts,
							   ImportContactsViewModel importContacts,
							   CreateContactViewModel createContact)
    {
		_myContacts = myContacts;
		_importContacts = importContacts;
		_createContact = createContact;
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
		Items.Add(_myContacts);
		Items.Add(_importContacts);
		Items.Add(_createContact);
	}
}
