using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess.DAO;

/// <summary>
/// Data Access Object for the Pipeline model
/// </summary>
/// <see cref="Pipeline"/>
public class PipelineDAO : IPipelineDAO
{
    private readonly ISqlDataAccess _db;

    public PipelineDAO(ISqlDataAccess db)
    {
        _db = db;
    }

    /// <summary>
    /// Get all pipelines
    /// </summary>
    /// <returns>All pipelines</returns>
    public async Task<IEnumerable<Pipeline>> GetPipelines() =>
        await _db.LoadData<Pipeline, dynamic>("dbo.spPipeline_GetAll", new { });

    /// <summary>
    /// Get pipeline by ID
    /// </summary>
    /// <param name="Id">ID for the query</param>
    /// <returns>One pipeline</returns>
    public async Task<Pipeline?> GetPipeline(string Id)
    {
        var result = await _db.LoadData<Pipeline, dynamic>("dbo.spPipeline_Get", new { Id });

        return result.FirstOrDefault();
    }

    /// <summary>
    /// Insert pipeline
    /// </summary>
    /// <param name="pipeline">Pipeline to be inserted</param>
    /// <returns></returns>
    public async Task InsertPipeline(Pipeline pipeline) =>
        await _db.SaveData("dbo.spPipeline_Insert", pipeline);

    /// <summary>
    /// Update pipeline
    /// </summary>
    /// <param name="pipeline">Pipeline to be updated</param>
    /// <returns></returns>
    public async Task UpdatePipeline(Pipeline pipeline) =>
        await _db.SaveData("dbo.spPipeline_Update", pipeline);

    /// <summary>
    /// Delete pipeline
    /// </summary>
    /// <param name="Id">ID for the query</param>
    /// <returns></returns>
    public async Task DeletePipeline(string Id) =>
        await _db.SaveData("dbo.spPipeline_Delete", new { Id });
}
