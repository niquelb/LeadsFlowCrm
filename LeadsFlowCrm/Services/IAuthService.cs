using Google.Apis.Auth.OAuth2;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services
{
	public interface IAuthService
	{
		Task<UserCredential> GetCredentials();
	}
}