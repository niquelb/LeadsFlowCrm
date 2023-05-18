﻿using DataAccess.DbAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess;

public class UserOrgRelationship : IUserOrgRelationship
{
	private readonly ISqlDataAccess _db;

	public UserOrgRelationship(ISqlDataAccess db)
	{
		_db = db;
	}

	public async Task SetupRelationship(string UserId, string OrgId) =>
		await _db.SaveData("spUserOrganization_Relationship", new { UserId, OrgId });
}
