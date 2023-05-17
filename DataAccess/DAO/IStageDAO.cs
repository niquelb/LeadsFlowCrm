using DataAccess.Models;

namespace DataAccess.DAO
{
	public interface IStageDAO
	{
		Task DeleteStage(string Id);
		Task<Stage?> GetStage(string Id);
		Task<IEnumerable<Stage>> GetStages();
		Task InsertStage(Stage stage);
		Task UpdateStage(Stage stage);
	}
}