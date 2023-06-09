using Google.Apis.PeopleService.v1;
using Google.Apis.PeopleService.v1.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services
{
	public interface IPeopleServiceClass
	{
		Task<List<Person>> GetPeopleAsync();
		Task<PeopleServiceService> GetPeopleServiceAsync();
	}
}