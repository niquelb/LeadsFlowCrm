﻿using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DataAccess.DAO;

/// <summary>
/// Data Access Object for the User model
/// </summary>
/// <see cref="User"/>
public class UserDAO : IUserDAO
{
    private readonly ISqlDataAccess _db;

    public UserDAO(ISqlDataAccess db)
    {
        _db = db;
    }

    /// <summary>
    /// Method for getting a specific User's ID with a given Email
    /// </summary>
    /// <param name="email">Email for the query</param>
    /// <returns></returns>
    public async Task<string?> GetUserByEmail(string email)
    {
        var result = await _db.LoadData<string, dynamic>("dbo.spUser_GetByEmail", new { email });

        return result.FirstOrDefault();
	}

    /// <summary>
    /// Get all users
    /// </summary>
    /// <returns>All users</returns>
    public async Task<IEnumerable<User>> GetUsers() =>
        await _db.LoadData<User, dynamic>("dbo.spUser_GetAll", new { });

    /// <summary>
    /// Get user by ID
    /// </summary>
    /// <param name="Id">ID for the query</param>
    /// <returns>One user</returns>
    public async Task<User?> GetUser(string Id)
    {
        var result = await _db.LoadData<User, dynamic>("dbo.spUser_Get", new { Id });

        return result.FirstOrDefault();
    }

    /// <summary>
    /// Insert user
    /// </summary>
    /// <param name="user">User to be inserted</param>
    /// <returns></returns>
    public async Task InsertUser(User user) =>
        await _db.SaveData("dbo.spUser_Insert", user);

    /// <summary>
    /// Update user
    /// </summary>
    /// <param name="user">User to be updated</param>
    /// <returns></returns>
    public async Task UpdateUser(User user) =>
        await _db.SaveData("dbo.spUser_Update", user);

    /// <summary>
    /// Delete user
    /// </summary>
    /// <param name="Id">ID for the query</param>
    /// <returns></returns>
    public async Task DeleteUser(string Id) =>
        await _db.SaveData("dbo.spUser_Delete", new { Id });

	/// <summary>
	/// Sets up the OrganizationId field with a given OrgId
	/// </summary>
	/// <param name="UserId">ID for the query</param>
	/// <param name="OrgId">ID for the query</param>
	/// <returns></returns>
	public async Task SetupUserOrgRelationship(string UserId, string OrgId) =>
		await _db.SaveData("spUserOrganization_Relationship", new { UserId, OrgId });
}
