namespace LeadsFlowApiV2.Auth
{
	public interface IAuthFilter
	{
		bool CheckApiKey(string apiKey);
	}
}