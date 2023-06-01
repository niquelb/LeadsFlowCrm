using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services
{
	public interface IBaseGoogleServiceClass
	{
		Task<UserCredential> GetCredentialsAsync();
		Task<BaseClientService.Initializer> GetServiceAsync();
		Task ReAuthorizeUser();
	}
}