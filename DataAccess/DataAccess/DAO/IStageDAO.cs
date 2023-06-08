using DataAccess.Models;

namespace DataAccess.DataAccess.DAO
{
    public interface IStageDAO
    {
        Task DeleteStage(string Id);
        Task<Stage?> GetStage(string Id);
        Task<IEnumerable<Stage>> GetStages(string? query = null);
        Task InsertStage(Stage stage);
        Task UpdateStage(Stage stage);
    }
}