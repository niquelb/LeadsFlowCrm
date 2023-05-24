using Google.Apis.Auth.OAuth2;
using LeadsFlowCrm.Models;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Utils
{
	public interface IApiHelper
	{
		Task Authenticate(string OauthToken, string UserName);
		Task<ClientSecrets?> GetGoogleClientSecrets();
	}
}