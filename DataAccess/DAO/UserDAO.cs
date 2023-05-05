using DataAccess.DbAccess;
using DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data;

/// <summary>
/// Data Access Object for the User model
/// </summary>
public class UserDAO : IUserDAO
{
	private readonly ISqlDataAccess _db;

	public UserDAO(ISqlDataAccess db)
	{
		_db = db;
	}

	/// <summary>
	/// Get all users
	/// </summary>
	/// <returns>All users</returns>
	public async Task<IEnumerable<User>> GetUsers() => await _db.LoadData<User, dynamic>("dbo.spUser_GetAll", new { });

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
	public async Task InsertUser(User user) => await _db.SaveData("dbo.spUser_Insert", user);

	/// <summary>
	/// Update user
	/// </summary>
	/// <param name="user">User to be updated</param>
	/// <returns></returns>
	public async Task UpdateUser(User user) => await _db.SaveData("dbo.spUser_Update", user);

	/// <summary>
	/// Delete user
	/// </summary>
	/// <param name="Id">Id to be used for deletion</param>
	/// <returns></returns>
	public async Task DeleteUser(string Id) => await _db.SaveData("dbo.spUser_Delete", new { Id });
}
