using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services
{
	public interface IGoogleService
	{
		Task<UserCredential> GetCredentialsAsync();

		Task<Oauth2Service> GetOauthServiceAsync();

		Task<GmailService> GetGmailServiceAsync();
	}
}