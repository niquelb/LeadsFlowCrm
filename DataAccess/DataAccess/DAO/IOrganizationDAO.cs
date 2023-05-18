using DataAccess.Models;

namespace DataAccess.DataAccess.DAO
{
    public interface IOrganizationDAO
    {
        Task DeleteOrganization(string Id);
        Task<Organization?> GetOrganization(string Id);
        Task<IEnumerable<Organization>> GetOrganizations();
        Task InsertOrganization(Organization organization);
        Task UpdateOrganization(Organization organization);
    }
}