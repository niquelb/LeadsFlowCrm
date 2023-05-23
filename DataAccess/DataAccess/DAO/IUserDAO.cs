using DataAccess.Models;

namespace DataAccess.DataAccess.DAO
{
    public interface IUserDAO
    {
        Task DeleteUser(string Id);
        Task<User?> GetUser(string Id);
        Task<string?> GetUserByEmail(string Email);
        Task<IEnumerable<User>> GetUsers();
        Task InsertUser(User user);
        Task UpdateUser(User user);
        Task SetupUserOrgRelationship(string UserId, string OrgId);

	}
}