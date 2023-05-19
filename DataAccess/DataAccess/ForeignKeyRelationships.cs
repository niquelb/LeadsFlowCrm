using DataAccess.DbAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess;

public class ForeignKeyRelationships : IForeignKeyRelationships
{
	private readonly ISqlDataAccess _db;

	public ForeignKeyRelationships(ISqlDataAccess db)
	{
		_db = db;
	}

	public async Task SetupUserOrgRelationship(string UserId, string OrgId) =>
		await _db.SaveData("spUserOrganization_Relationship", new { UserId, OrgId });

	public async Task SetupOrgCreatorRelationship(string OrgId, string UserId) =>
		await _db.SaveData("spOrganizationCreator_Relationship", new { OrgId, UserId });

	public async Task SetupPipelineUserRelationship(string UserId, string PipelineId) =>
		await _db.SaveData("spPipelineUser_Relationship", new { UserId, PipelineId });

	public async Task SetupStagePipelineRelathionship(string PipelineId, string StageId) =>
		await _db.SaveData("spStagePipeline_Relationship", new { PipelineId, StageId });

	public async Task SetupFieldPipelineRelathionship(string PipelineId, string FieldId) =>
		await _db.SaveData("spFieldPipeline_Relationship", new { PipelineId, FieldId });
}
