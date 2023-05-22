using Google.Apis.Auth.OAuth2;
using LeadsFlowCrm.Models;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Utils
{
	public interface IApiHelper
	{
		void Authenticate(string OauthToken);
		void Dispose();
		Task<ClientSecrets?> GetGoogleClientSecrets();
	}
}