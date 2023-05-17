using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAO;

/// <summary>
/// Data Access Object for the Stage model
/// </summary>
/// <see cref="Stage"/>
public class StageDAO : IStageDAO
{
	private readonly ISqlDataAccess _db;

	public StageDAO(ISqlDataAccess db)
	{
		_db = db;
	}

	/// <summary>
	/// Get all stages
	/// </summary>
	/// <returns>All stages</returns>
	public async Task<IEnumerable<Stage>> GetStages() =>
		await _db.LoadData<Stage, dynamic>("dbo.spStage_GetAll", new { });

	/// <summary>
	/// Get stage by ID
	/// </summary>
	/// <param name="Id">ID for the query</param>
	/// <returns>One stage</returns>
	public async Task<Stage?> GetStage(string Id)
	{
		var result = await _db.LoadData<Stage, dynamic>("dbo.spStage_Get", new { Id });

		return result.FirstOrDefault();
	}

	/// <summary>
	/// Insert stage
	/// </summary>
	/// <param name="stage">Stage to be inserted</param>
	/// <returns></returns>
	public async Task InsertStage(Stage stage) =>
		await _db.SaveData("dbo.spStage_Insert", stage);

	/// <summary>
	/// Update stage
	/// </summary>
	/// <param name="stage">stage to be updated</param>
	/// <returns></returns>
	public async Task UpdateStage(Stage stage) =>
		await _db.SaveData("dbo.spStage_Update", stage);

	/// <summary>
	/// Delete stage
	/// </summary>
	/// <param name="Id">ID for the query</param>
	/// <returns></returns>
	public async Task DeleteStage(string Id) =>
		await _db.SaveData("dbo.spStage_Delete", new { Id });
}
