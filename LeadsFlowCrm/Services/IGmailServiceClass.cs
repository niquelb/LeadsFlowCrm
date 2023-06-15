using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using LeadsFlowCrm.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using static LeadsFlowCrm.Services.GmailServiceClass;

namespace LeadsFlowCrm.Services
{
	public interface IGmailServiceClass
	{
		Email? SelectedEmail { get; set; }
		Task<IList<Email>> GetInboxAsync(PaginationOptions paginationOption = PaginationOptions.FirstPage);
		Task<IList<Email>> GetDraftsAsync(PaginationOptions paginationOption = PaginationOptions.FirstPage);
		Task<GmailService> GetGmailServiceAsync();
		string GetProcessedBody(Email email);
		Task MarkEmailAsReadAsync(Email email);
		Task MarkEmailAsUnreadAsync(Email email);
		Task MarkEmailAsArchivedAsync(Email email);
		Task MarkEmailAsTrashAsync(Email email);
		Task MarkEmailAsFavoriteAsync(Email email);
		Task MarkEmailAsNotFavoriteAsync(Email email);
		Task SendEmailAsync(Email email);
		Task SaveDraftAsync(Email email);
		Task SendDraftAsync(Email email);
		Task DeleteDraftAsync(Email email);
		Task<IList<Email>> GetAllMailAsync(PaginationOptions paginationOptions = PaginationOptions.FirstPage, bool includeTrashSpam = false);
	}
}
