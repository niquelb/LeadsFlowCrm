using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess.DAO;

/// <summary>
/// Data Access Object for the Organization model
/// </summary>
/// <see cref="Organization"/>
public class OrganizationDAO : IOrganizationDAO
{
    private readonly ISqlDataAccess _db;

    public OrganizationDAO(ISqlDataAccess db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all organizations
    /// </summary>
    /// <returns>All organizations</returns>
    public async Task<IEnumerable<Organization>> GetOrganizations() =>
        await _db.LoadData<Organization, dynamic>("dbo.spOrganization_GetAll", new { });

    /// <summary>
    /// Get organization by ID
    /// </summary>
    /// <param name="Id">ID for the query</param>
    /// <returns>One organization</returns>
    public async Task<Organization?> GetOrganization(string Id)
    {
        var result = await _db.LoadData<Organization, dynamic>("dbo.spOrganization_Get", new { Id });

        return result.FirstOrDefault();
    }

    /// <summary>
    /// Insert organization
    /// </summary>
    /// <param name="organization">Organization to be inserted</param>
    /// <returns></returns>
    public async Task InsertOrganization(Organization organization) =>
        await _db.SaveData("dbo.spOrganization_Insert", organization);

    /// <summary>
    /// Update organization
    /// </summary>
    /// <param name="organization">Organization to be updated</param>
    /// <returns></returns>
    public async Task UpdateOrganization(Organization organization) =>
        await _db.SaveData("dbo.spOrganization_Update", organization);

    /// <summary>
    /// Delete organization
    /// </summary>
    /// <param name="Id">ID for the query</param>
    /// <returns></returns>
    public async Task DeleteOrganization(string Id) =>
        await _db.SaveData("dbo.spOrganization_Delete", new { Id });
}
