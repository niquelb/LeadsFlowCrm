using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO;

public class OrganizationDAO : IOrganizationDAO
{
	// TODO: Comment methods
	private readonly ISqlDataAccess _db;

	public OrganizationDAO(ISqlDataAccess db)
	{
		_db = db;
	}

	public async Task<IEnumerable<Organization>> GetOrganizations() =>
		await _db.LoadData<Organization, dynamic>("dbo.spOrganization_GetAll", new { });

	public async Task<Organization?> GetOrganization(string Id)
	{
		var result = await _db.LoadData<Organization, dynamic>("dbo.spOrganization_Get", new { Id });

		return result.FirstOrDefault();
	}

	public async Task InsertOrganization(Organization organization) =>
		await _db.SaveData("dbo.spOrganization_Insert", organization);

	public async Task UpdateOrganization(Organization organization) =>
		await _db.SaveData("dbo.spOrganization_Update", organization);

	public async Task DeleteOrganization(string Id) =>
		await _db.SaveData("dbo.spOrganization_Delete", new { Id });
}
