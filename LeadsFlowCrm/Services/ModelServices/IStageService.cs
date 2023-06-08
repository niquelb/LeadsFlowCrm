using LeadsFlowCrm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services.ModelServices
{
	public interface IStageService
	{
		Task<IList<Stage>> GetStagesByPipelineAsync(string pipelineId);
	}
}