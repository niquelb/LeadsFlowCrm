using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO;

/// <summary>
/// Data Access Object for the Contact model
/// </summary>
/// <see cref="Contact"/>
public class ContactDAO : IContactDAO
{
	private readonly ISqlDataAccess _db;

	public ContactDAO(ISqlDataAccess db)
	{
		_db = db;
	}

	/// <summary>
	/// Get all contacts
	/// </summary>
	/// <returns>All contacts</returns>
	public async Task<IEnumerable<Contact>> GetContacts() =>
		await _db.LoadData<Contact, dynamic>("dbo.spContact_GetAll", new { });

	/// <summary>
	/// Get contact by ID
	/// </summary>
	/// <param name="Id">ID for the query</param>
	/// <returns>One contact</returns>
	public async Task<Contact?> GetContact(string Id)
	{
		var result = await _db.LoadData<Contact, dynamic>("dbo.spContact_Get", new { Id });

		return result.FirstOrDefault();
	}

	/// <summary>
	/// Insert contact
	/// </summary>
	/// <param name="contact">Contact to be inserted</param>
	/// <returns></returns>
	public async Task InsertContact(Contact contact) =>
		await _db.SaveData("dbo.spContact_Insert", contact);

	/// <summary>
	/// Update contact
	/// </summary>
	/// <param name="contact">Contact to be updated</param>
	/// <returns></returns>
	public async Task UpdateContact(Contact contact) =>
		await _db.SaveData("dbo.spContact_Update", contact);

	/// <summary>
	/// Delete contact
	/// </summary>
	/// <param name="Id">ID for the query</param>
	/// <returns></returns>
	public async Task DeleteContact(string Id) =>
		await _db.SaveData("dbo.spContact_Delete", new { Id });
}
