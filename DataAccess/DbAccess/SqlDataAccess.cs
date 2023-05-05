using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace DataAccess.DbAccess;

public class SqlDataAccess : ISqlDataAccess
{
	private readonly IConfiguration _config;

	public SqlDataAccess(IConfiguration config)
	{
		_config = config;
	}

	/// <summary>
	/// Method for retrieving data from the DB using a Stored Procedure
	/// </summary>
	/// <typeparam name="T">Generic</typeparam>
	/// <typeparam name="U">Generic</typeparam>
	/// <param name="storedProcedure">Stored Procedure for the query</param>
	/// <param name="parameters">Optional parameters for the query</param>
	/// <param name="connectionId">Connection string, default is "Default"</param>
	/// <returns>Query response</returns>
	public async Task<IEnumerable<T>> LoadData<T, U>(string storedProcedure, U parameters, string connectionId = "Default")
	{
		// Get the connection object and make sure it's disposed of with using
		using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString(connectionId));

		// Make the query
		return await dbConnection.QueryAsync<T>(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
	}

	/// <summary>
	/// Method for uploading data to thee DB using a Stored Procedure
	/// </summary>
	/// <typeparam name="T">Generic</typeparam>
	/// <param name="storedProcedure">Stored Procedure for the query</param>
	/// <param name="parameters">Optional parameters for the query</param>
	/// <param name="connectionId">Connection string, default is "Default"</param>
	/// <returns></returns>
	public async Task SaveData<T>(string storedProcedure, T parameters, string connectionId = "Default")
	{
		// Get the connection object and make sure it's disposed of with using
		using IDbConnection dbConnection = new SqlConnection(_config.GetConnectionString(connectionId));

		// Make the query
		await dbConnection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
	}
}
