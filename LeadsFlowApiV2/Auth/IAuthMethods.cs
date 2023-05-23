namespace LeadsFlowApiV2.Auth
{
	public interface IAuthMethods
	{
		bool CheckApiKey(string apiKey);

		Task<bool> CheckOauthToken(string token);

		string GetToken(string oauth, string email, string id);
	}
}