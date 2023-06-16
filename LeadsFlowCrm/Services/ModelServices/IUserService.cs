using LeadsFlowCrm.Models;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services.ModelServices
{
	public interface IUserService
	{
		Task AuthenticateAsync(string oAuthToken, string email, string name);
		Task<User> GetUserAsync(string id, string token);
		Task<bool> GoogleSignInAsync();
		Task LogoutUserAsync();
	}
}