using Google.Apis.Auth.OAuth2;
using LeadsFlowCrm.Models;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services.ModelServices
{
	public interface IUserService
	{
		Task AuthenticateAsync(string OAuthToken, string Email);
		Task<User> GetUserAsync(string id, string token);
	}
}