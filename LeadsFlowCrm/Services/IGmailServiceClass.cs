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
		Task MarkEmailAsReadAsync(Email email);
		Task MarkEmailAsUnreadAsync(Email email);
		Task MarkEmailAsArchivedAsync(Email email);
		Task MarkEmailAsTrashAsync(Email email);
		Task MarkEmailAsFavoriteAsync(Email email);
		Task MarkEmailAsNotFavoriteAsync(Email email);
		Task RefreshInboxAsync();
		Task<List<Email>> RefreshAndGetInboxAsync();
		Task SendEmailAsync(Email email);
		Task SaveDraftAsync(Email email);
		Task<IList<Email>> GetPaginatedInbox(GmailServiceClass.PaginationOptions paginationOption);
		Task<IList<Email>> GetDraftsAsync();
	}
}
