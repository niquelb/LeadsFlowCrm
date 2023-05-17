using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO;

/// <summary>
/// Data Access Object for the Field model
/// </summary>
/// <see cref="Field"/>
public class FieldDAO : IFieldDAO
{
	private readonly ISqlDataAccess _db;

	public FieldDAO(ISqlDataAccess db)
	{
		_db = db;
	}

	/// <summary>
	/// Get all fields
	/// </summary>
	/// <returns>All fields</returns>
	public async Task<IEnumerable<Field>> GetFields() =>
		await _db.LoadData<Field, dynamic>("dbo.spField_GetAll", new { });

	/// <summary>
	/// Get field by ID
	/// </summary>
	/// <param name="Id">ID for the query</param>
	/// <returns>One field</returns>
	public async Task<Field?> GetField(string Id)
	{
		var result = await _db.LoadData<Field, dynamic>("dbo.spField_Get", new { Id });

		return result.FirstOrDefault();
	}

	/// <summary>
	/// Insert field
	/// </summary>
	/// <param name="field">Field to be inserted</param>
	/// <returns></returns>
	public async Task InsertField(Field field) =>
		await _db.SaveData("dbo.spField_Insert", field);

	/// <summary>
	/// Update field
	/// </summary>
	/// <param name="field">Field to be updated</param>
	/// <returns></returns>
	public async Task UpdateField(Field field) =>
		await _db.SaveData("dbo.spField_Update", field);

	/// <summary>
	/// Delete field
	/// </summary>
	/// <param name="Id">ID for the query</param>
	/// <returns></returns>
	public async Task DeleteField(string Id) =>
		await _db.SaveData("dbo.spField_Delete", new { Id });
}
