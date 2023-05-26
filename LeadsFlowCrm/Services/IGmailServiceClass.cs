using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using LeadsFlowCrm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services
{
	public interface IGmailServiceClass
	{
		Email? SelectedEmail { get; set; }

		Task<List<Email>> GetInboxAsync();

		Task<GmailService> GetGmailServiceAsync();

		string GetProcessedBody(Email email);
	}
}