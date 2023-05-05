using DataAccess.Models;

namespace DataAccess.Data
{
	public interface IUserDAO
	{
		Task DeleteUser(string Id);
		Task<User?> GetUser(string Id);
		Task<IEnumerable<User>> GetUsers();
		Task InsertUser(User user);
		Task UpdateUser(User user);
	}
}