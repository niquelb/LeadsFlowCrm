using Google.Apis.Gmail.v1;
using LeadsFlowCrm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services
{
	public interface IGmailServiceClass
	{
		Task<List<Email>> GetEmailsFromInboxAsync();
		Task<GmailService> GetGmailServiceAsync();
	}
}