using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO;

public class PipelineDAO : IPipelineDAO
{
	private readonly ISqlDataAccess _db;

	public PipelineDAO(ISqlDataAccess db)
	{
		_db = db;
	}

	public async Task<IEnumerable<Pipeline>> GetPipelines() =>
		await _db.LoadData<Pipeline, dynamic>("dbo.spPipeline_GetAll", new { });

	public async Task<Pipeline?> GetPipeline(string Id)
	{
		var result = await _db.LoadData<Pipeline, dynamic>("dbo.spPipeline_Get", new { Id });

		return result.FirstOrDefault();
	}

	public async Task InsertPipeline(Pipeline pipeline) =>
		await _db.SaveData("dbo.spPipeline_Insert", pipeline);

	public async Task UpdatePipeline(Pipeline pipeline) =>
		await _db.SaveData("dbo.spPipeline_Update", pipeline);

	public async Task DeletePipeline(string Id) =>
		await _db.SaveData("dbo.spPipeline_Delete", new { Id });
}
