namespace LeadsFlowApiV2.Auth
{
	public interface IAuthMethods
	{
		bool CheckApiKey(string apiKey);

		Task UpdateUser(string id, string oAuthToken);

		Task<bool> CheckOauthToken(string token);

		string GetToken(string id);
	}
}