using LeadsFlowCrm.Models;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services.ModelServices
{
	public interface IUserService
	{
		Task AuthenticateAsync(string OAuthToken, string Email);
		Task<User> GetUserAsync(string id, string token);
		Task<bool> GoogleSignInAsync();
		Task LogoutUserAsync();
	}
}