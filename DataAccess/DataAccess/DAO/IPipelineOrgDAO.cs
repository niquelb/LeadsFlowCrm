using DataAccess.Models;

namespace DataAccess.DataAccess.DAO
{
    public interface IPipelineOrgDAO
    {
        Task DeletePipelineOrg(string Id);
        Task<PipelineOrganization?> GetPipelineOrg(string Id);
        Task<IEnumerable<PipelineOrganization>> GetPipelineOrgs();
        Task InsertPipelineOrg(PipelineOrganization pipelineOrg);
        Task UpdatePipelineOrg(PipelineOrganization pipelineOrg);
    }
}