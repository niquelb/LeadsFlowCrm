using Google.Apis.Auth.OAuth2;
using LeadsFlowCrm.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Utils
{
	public interface IApiHelper
	{
		HttpClient ApiClient { get; set; }

		Task<ClientSecrets?> GetGoogleClientSecretsAsync();
	}
}