namespace LeadsFlowApiV2.Auth
{
	public interface IAuthMethods
	{
		bool CheckApiKey(string apiKey);

		string GetToken(string oauth, string username);
	}
}