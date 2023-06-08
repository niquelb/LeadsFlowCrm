using LeadsFlowCrm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services.ModelServices
{
	public interface IContactService
	{
		Task<IList<Contact>> GetAllAsync();
		Task<IList<Contact>> GetByStageAsync(string stageId);
	}
}