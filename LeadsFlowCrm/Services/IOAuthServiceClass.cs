using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Oauth2.v2;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services
{
	public interface IOAuthServiceClass
	{
		Task<Oauth2Service> GetOauthServiceAsync();

	}
}