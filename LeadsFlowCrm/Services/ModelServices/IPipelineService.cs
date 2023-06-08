using LeadsFlowCrm.Models;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services.ModelServices
{
	public interface IPipelineService
	{
		Task<Pipeline> GetPipelineAsync();
	}
}