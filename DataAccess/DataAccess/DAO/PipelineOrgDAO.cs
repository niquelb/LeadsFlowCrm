using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess.DAO;

/// <summary>
/// Data Access Object for the PipelineOrganization model
/// </summary>
/// <see cref="PipelineOrganization"/>
public class PipelineOrgDAO : IPipelineOrgDAO
{
    private readonly ISqlDataAccess _db;

    public PipelineOrgDAO(ISqlDataAccess db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all pipelineOrgs
    /// </summary>
    /// <returns>All pipelineOrgs</returns>
    public async Task<IEnumerable<PipelineOrganization>> GetPipelineOrgs() =>
        await _db.LoadData<PipelineOrganization, dynamic>("dbo.spPipelineOrganization_GetAll", new { });

    /// <summary>
    /// Get pipelineOrg by ID
    /// </summary>
    /// <param name="Id">ID for the query</param>
    /// <returns>One pipelineOrgs</returns>
    public async Task<PipelineOrganization?> GetPipelineOrg(string Id)
    {
        var result = await _db.LoadData<PipelineOrganization, dynamic>("dbo.spPipelineOrganization_Get", new { Id });

        return result.FirstOrDefault();
    }

    /// <summary>
    /// Insert pipelineOrg
    /// </summary>
    /// <param name="pipelineOrg">PipelineOrg to be inserted</param>
    /// <returns></returns>
    public async Task InsertPipelineOrg(PipelineOrganization pipelineOrg) =>
        await _db.SaveData("dbo.spPipelineOrganization_Insert", pipelineOrg);

    /// <summary>
    /// Update pipelineOrg
    /// </summary>
    /// <param name="pipelineOrg">PipelineOrg to be updated</param>
    /// <returns></returns>
    public async Task UpdatePipelineOrg(PipelineOrganization pipelineOrg) =>
        await _db.SaveData("dbo.spPipelineOrganization_Update", pipelineOrg);

    /// <summary>
    /// Delete pipelineOrg
    /// </summary>
    /// <param name="Id">ID for the query</param>
    /// <returns></returns>
    public async Task DeletePipelineOrg(string Id) =>
        await _db.SaveData("dbo.spPipelineOrganization_Delete", new { Id });
}
