using DataAccess.Models;

namespace DataAccess.DataAccess.DAO
{
    public interface IPipelineDAO
    {
        Task DeletePipeline(string Id);
        Task<Pipeline?> GetPipeline(string Id);
        Task<IEnumerable<Pipeline>> GetPipelines();
        Task InsertPipeline(Pipeline pipeline);
        Task UpdatePipeline(Pipeline pipeline);
    }
}