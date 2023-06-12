using LeadsFlowCrm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services.ModelServices
{
	public interface IContactService
	{
		Task<IList<Contact>> GetByUserAsync(string userId);
		Task<IList<Contact>> GetByStageAsync(string stageId);
		Task<IList<Contact>> GetFromPeopleApiAsync();
		Task PostToApiAsync(Contact contact, string UserId, string? StageId = null);
	}
}