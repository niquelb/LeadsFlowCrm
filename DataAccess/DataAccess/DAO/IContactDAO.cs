using DataAccess.Models;

namespace DataAccess.DataAccess.DAO
{
    public interface IContactDAO
    {
        Task DeleteContact(string Id);
        Task<Contact?> GetContact(string Id);
        Task<IEnumerable<Contact>> GetContacts(string? query = null);
        Task InsertContact(Contact contact);
        Task UpdateContact(Contact contact);
    }
}