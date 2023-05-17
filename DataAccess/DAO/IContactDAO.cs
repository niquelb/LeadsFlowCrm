using DataAccess.Models;

namespace DataAccess.DAO
{
	public interface IContactDAO
	{
		Task DeleteContact(string Id);
		Task<Contact?> GetContact(string Id);
		Task<IEnumerable<Contact>> GetContacts();
		Task InsertContact(Contact contact);
		Task UpdateContact(Contact contact);
	}
}