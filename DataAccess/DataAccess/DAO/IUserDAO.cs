using DataAccess.Models;

namespace DataAccess.DataAccess.DAO
{
    public interface IUserDAO
    {
        Task DeleteUser(string Id);
        Task<User?> GetUser(string Id);
        Task<IEnumerable<User>> GetUsers(string? query = null);
        Task InsertUser(User user);
        Task UpdateUser(User user);
        Task SetupUserOrgRelationship(string UserId, string OrgId);

	}
}