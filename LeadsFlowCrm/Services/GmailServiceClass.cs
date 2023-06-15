using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LeadsFlowCrm.Models;
using LeadsFlowCrm.Utils;
using System.Globalization;
using System.Diagnostics;
using System.Windows.Interop;

namespace LeadsFlowCrm.Services;

public class GmailServiceClass : IGmailServiceClass
{
	#region private fields
	private readonly IOAuthServiceClass _oAuthService;
	private readonly IBaseGoogleServiceClass _baseGoogleService;

	/// <summary> Service object for the Gmail API </summary>
	private GmailService? _gmailService;
	#endregion

	public GmailServiceClass(IBaseGoogleServiceClass baseGoogleService, IOAuthServiceClass oAuthService)
	{
		_baseGoogleService = baseGoogleService;
		_oAuthService = oAuthService;
	}

	#region Enums

	/// <summary>
	/// Pagination options available
	/// </summary>
	public enum PaginationOptions
	{
		PreviousPage,
		NextPage,
		FirstPage,
	}
	#endregion

	#region Properties
	/// <summary> Currently selected email </summary>
	public Email? SelectedEmail { get; set; }

	/// <summary> Special keyword reserved for referencing the logged in user </summary>
	public static string Me => "me";

	/// <summary> Number of emails per page </summary>
	public static int PageCount => 50;

    /// <summary> List of pagination tokens for the Gmail API </summary>
    private List<string> PageTokens { get; set; } = new();

	#region Labels
	/// <summary>
	/// Label to get the emails from the inbox
	/// </summary>
	/// <see cref="GetEmailsFromUserAsync(PaginationOptions)"/>
	public const string InboxLabel = "INBOX";

	/// <summary>
	/// Empty label to get all mail
	/// </summary>
	/// <see cref="GetEmailsFromUserAsync(PaginationOptions)"/>
	public const string NoLabel = "";
	#endregion

	#endregion

	#region Get and processing methods

	/// <summary>
	/// Method for retrieving the Gmail service object for the Gmail API
	/// </summary>
	/// <returns>GmailService object</returns>
	/// <see cref="GmailService"/>
	public async Task<GmailService> GetGmailServiceAsync()
	{
		_gmailService ??= new GmailService(await _baseGoogleService.GetServiceAsync());

		return _gmailService;
	}

	/// <summary>
	/// Method for retrieving the emails from the user's inbox
	/// </summary>
	/// <param name="paginationOption">Optional pagination options</param>
	/// <returns>List of the emails in the user's inbox</returns>
	public async Task<IList<Email>> GetInboxAsync(PaginationOptions paginationOption = PaginationOptions.FirstPage) => 
		await GetEmailsFromUserAsync(pagination: paginationOption, label: InboxLabel);

	/// <summary>
	/// Method for retrieving the drafts created by the user
	/// </summary>
	/// <param name="paginationOption">Optional pagination options</param>
	/// <returns>List of user's drafts</returns>
	public async Task<IList<Email>> GetDraftsAsync(PaginationOptions paginationOption = PaginationOptions.FirstPage) => 
		await GetDraftsFromUserAsync(pagination: paginationOption);

	/// <summary>
	/// Method for retrieving all the user's emails
	/// </summary>
	/// <param name="paginationOption">Optional pagination options</param>
	/// <returns></returns>
	public async Task<IList<Email>> GetAllMailAsync(PaginationOptions paginationOptions = PaginationOptions.FirstPage, bool includeTrashSpam = false) =>
		await GetEmailsFromUserAsync(pagination: paginationOptions, NoLabel, includeSpamTrash: includeTrashSpam);

	/// <summary>
	/// Method for getting the processed and unencoded body of the selected email
	/// </summary>
	/// <returns></returns>
	public string GetProcessedBody(Email email)
	{
		if (email == null)
		{
			return string.Empty;
		}

		// We save the encoded body to the object in case we need it in the future
		email.EncodedBody = GetEncodedBody(email.Message);

		try
		{
			return Utilities.Base64Decode(email.EncodedBody);
		}
		catch (Exception ex)
		{
			Trace.TraceError(ex.Message);
			return string.Empty;
		}
	}

	/// <summary>
	/// Method for getting the emails from the user's inbox
	/// </summary>
	/// <param name="pagination">Optional pagination options</param>
	/// <param name="label">Label for the emails, by default the label is "INBOX"</param>
	/// <param name="includeSpamTrash">Wether or not to include emails marked as trash or spam, by default it's set to false</param>
	/// <returns>List of emails from the user's inbox</returns>
	private async Task<IList<Email>> GetEmailsFromUserAsync(PaginationOptions pagination = PaginationOptions.FirstPage,
														 string label = InboxLabel,
														 bool includeSpamTrash = false)
	{
		List<Task<Email>> tasks = new();

		// We get the service and the user's email
		GmailService gmailService = await GetGmailServiceAsync();

		// We define the request to retrieve email list
		var emailListRequest = gmailService.Users.Messages.List(Me);

		// Query
		emailListRequest.Q = "in:all -from:me";

		// Only get emails with the requested label
		if (string.IsNullOrWhiteSpace(label) == false) // ← If there are no labels, all emails will be returned (matching the previous query)
		{
			emailListRequest.LabelIds = new[] { label };
		}

        // Not include spam/trash
        emailListRequest.IncludeSpamTrash = includeSpamTrash;

		// Specify the page size
		emailListRequest.MaxResults = PageCount;

		// We pass in the page token depending on the desired pagination
		emailListRequest.PageToken = GetPageToken(pagination);

		// Execute the request to retrieve the list of emails
		ListMessagesResponse emailListResponse = emailListRequest.Execute();

		// If there are no emails in the inbox or there was an error
		if (emailListResponse == null || emailListResponse.Messages == null)
		{
			return new List<Email>();
		}

		// For the pagination, we only save the NextPageToken if it's a new page
        if (string.IsNullOrWhiteSpace(emailListResponse.NextPageToken) == false)
        {
			bool isSame = false;

            foreach (var t in PageTokens)
            {
                if (emailListResponse.NextPageToken.Equals(t))
                {
                    isSame = true;
                }
            }

			if (isSame == false)
			{
				PageTokens.Add(emailListResponse.NextPageToken);
			}
		}

        // We process each email asynchronously
        foreach (Message email in emailListResponse.Messages)
		{
			tasks.Add(ProcessEmailAsync(email, gmailService));
		}

		/*
		 * We end up with list of tasks that will be executing in parallel, we await them.
		 * This will return an array of emails.
		 */
		var output = await Task.WhenAll(tasks);

		return new List<Email>(output);
	}

	/// <summary>
	/// Method for getting the drafts created by the user
	/// </summary>
	/// <param name="pagination">Optional pagination options</param>
	/// <returns></returns>
	private async Task<IList<Email>> GetDraftsFromUserAsync(PaginationOptions pagination = PaginationOptions.FirstPage)
	{
		List<Task<Email>> tasks = new();

		// We get the service and the user's email
		GmailService gmailService = await GetGmailServiceAsync();

		// We define the request to retrieve email list
		var emailListRequest = gmailService.Users.Drafts.List(Me);

		// Specify the page size
		emailListRequest.MaxResults = PageCount;

		// We pass in the page token depending on the desired pagination
		emailListRequest.PageToken = GetPageToken(pagination);

		// Execute the request to retrieve the list of emails
		var emailListResponse = emailListRequest.Execute();

		// If there are no emails in the inbox or there was an error
		if (emailListResponse == null || emailListResponse.Drafts == null)
		{
			return new List<Email>();
		}

		// For the pagination, we only save the NextPageToken if it's a new page
		if (string.IsNullOrWhiteSpace(emailListResponse.NextPageToken) == false)
		{
			bool isSame = false;

			foreach (var t in PageTokens)
			{
				if (emailListResponse.NextPageToken.Equals(t))
				{
					isSame = true;
				}
			}

			if (isSame == false)
			{
				PageTokens.Add(emailListResponse.NextPageToken);
			}
		}

		// We process each email asynchronously
		foreach (Draft draft in emailListResponse.Drafts)
		{
			tasks.Add(ProcessEmailAsync(draft.Message, gmailService, draft.Id));
		}

		/*
		 * We end up with list of tasks that will be executing in parallel, we await them.
		 * This will return an array of emails.
		 */
		var output = await Task.WhenAll(tasks);

		return new List<Email>(output);
	}

	/// <summary>
	/// Method for processing the body of a given Message object and return the email body STILL ENCODED IN BASE 64
	/// </summary>
	/// <param name="email">Message object</param>
	/// <returns>Email body encoded in base 64</returns>
	private static string GetEncodedBody(Message email)
	{
		if (email == null)
		{
			return string.Empty;
		}

		string output = string.Empty;

		/*
		 * Email bodies can follow 2 very different structures:
		 * 
		 *	- The body can be in the Payload itself, Email->Payload->Body
		 *	- Or the body can be inside the "Parts", Email->Parts[]->Body
		 */
		if (email.Payload.Parts == null)
		{
			// If the body is not inside the parts
			output = email.Payload.Body.Data;
		}
		else
		{
			// If the body is inside the parts
			foreach (var part in email.Payload.Parts)
			{

				/*
				 * The parts also include things like attachments, so we need to
				 * iterate through them to find the one with the body.
				 * 
				 * Not only that but parts can also have parts inside them.
				 * 
				 * Thanks Google for making this so complicated.
				 */

				if (part.Body != null && part.Body.Data != null)
				{
					// If the part contains the body
					output = part.Body.Data;

					break;
				}
				else if (part.Parts != null)
				{
					// If the part contains more parts
					foreach (var nestedPart in part.Parts)
					{
						if (nestedPart.Body != null && nestedPart.Body.Data != null)
						{
							output = nestedPart.Body.Data;
						}
					}
				}
			}

		}

		return output;
	}

	/// <summary>
	/// Method for processing a given Message object into an Email object
	/// </summary>
	/// <param name="email">Message to process</param>
	/// <param name="gmailService">GmailService object</param>
	/// <param name="userId">User's email</param>
	/// <returns>Processed Email object</returns>
	private static async Task<Email> ProcessEmailAsync(Message email, GmailService gmailService, string? draftId = null)
	{
		Email output = new();

		/*
		 * Retrieve the email details, this will create another "Message" object with the whole email with all the details
		 */
		var request = gmailService.Users.Messages.Get(Me, email.Id);
		request.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;
		request.MetadataHeaders = new string[] { "labelIds" };

		Message emailContent = await request.ExecuteAsync();

		/*
		 * We associate this email with the Email model
		 */
		output.Message = emailContent;

		/*
		 * The email labels (starred, unread...), these can be null
		 */
		var labels = emailContent.LabelIds;

		/*
		 * If the email is "starred" we mark it as favorite
		 */
		output.IsFavorite = labels != null && labels.Contains("STARRED");

		/*
		 * We check if the email is read or unread
		 */
		output.IsRead = labels != null && !(labels.Contains("UNREAD"));

		/*
		 * We parse the snippet, this is the part of the body visible in the inbox
		 */
		output.Snippet = emailContent.Snippet;

		/*
		 * We parse the internal date, if for some reason it is null the current date is parsed
		 */
		DateTimeOffset dtfInternal = DateTimeOffset.FromUnixTimeMilliseconds(
			emailContent.InternalDate ?? DateTimeOffset.Now.ToUnixTimeMilliseconds());

		output.InternalDate = dtfInternal.DateTime;

		/*
		 * Read the headers
		 */
		foreach (var header in emailContent.Payload.Headers)
		{
			switch (header.Name)
			{
				/*
				 * Recipient, this is for the emails/drafts sent by the user
				 */
				case "To":
					string toHeader = header.Value;

					// Extract the email address from the 'To' header
					string? recipient = toHeader?.Split(',')[0]?.Trim(); // Extract the first email address if multiple recipients are present

					output.To = recipient ?? string.Empty;
					break;
				/*
				 * Sender, the standard format is "name <email>" which we break down into "From" and "FromEmail"
				 */
				case "From":
					string fromHeader = header.Value;

					// Extracting the name and email from the "From" header
					int nameStartIndex = fromHeader.IndexOf('<');
					int nameEndIndex = nameStartIndex > 0 ? nameStartIndex - 1 : -1;

					output.From = nameEndIndex >= 0 ? fromHeader.Substring(0, nameEndIndex).Trim() : string.Empty;
					output.FromEmail = fromHeader.Substring(nameStartIndex + 1, fromHeader.Length - nameStartIndex - 2).Trim();
					break;
				/*
				 * Date of the email that will be displayed, can difer from the internal date
				 */
				case "Date":
					string dateString = header.Value;
					string[] format = { "ddd, dd MMM yyyy HH:mm:ss zzzz" };
					try
					{
						DateTimeOffset dtf = DateTimeOffset.ParseExact(dateString, format, CultureInfo.InvariantCulture);
						output.Date = dtf.DateTime;
					}
					catch (Exception)
					{
						output.Date = DateTime.Now;
					}
					break;
				/*
				 * Subject line
				 */
				case "Subject":
					output.SubjectLine = header.Value;
					break;
				default:
					break;
			}
		}

		/*
		 * If the email is a draft we save the id passed in as a parameter
		 * If none was passed in this will be null, that's completely fine
		 */
		output.DraftId = draftId;

        return output;
	}

	/// <summary>
	/// Method for getting the appropiate PageToken for the request
	/// </summary>
	/// <see cref="PageTokens"/>
	/// <param name="pagination"></param>
	/// <returns></returns>
	private string GetPageToken(PaginationOptions pagination)
	{
		string output = string.Empty;

		/*
		 * GMAIL API PAGINATION:
		 * Each request returns the token to retrieve the next one
		 * 
		 * If we want to retrieve the first page we send an empty string or null as the PageToken
		 * If we want to retrieve the next page we send the last PageToken we've recieved
		 * If we want to retrieve the current page we send the second to last PageToken we've recieved
		 * If we want to retrieve the previous page we send the third to last PageToken we've recieved
		 * 
		 * The way we go about this is by storing each token both in a collection of all retrieved tokens and 
		 * 
		 */

		switch (pagination)
		{
			case PaginationOptions.PreviousPage:
				// We remove the last inserted token
				PageTokens.Remove(PageTokens.LastOrDefault() ?? "");

                if (PageTokens.Count < 2)
                {
					output = string.Empty;

					PageTokens.Clear();
					break;
				}

				output = PageTokens[^2];

				break;
			case PaginationOptions.NextPage:
				output = PageTokens.LastOrDefault() ?? "";
				break;
			case PaginationOptions.FirstPage:
				output = string.Empty;

				PageTokens.Clear();
				break;
		}

		return output;
	}
	#endregion

	#region Email marking methods (favorites...)
	/// <summary>
	/// Method for marking a specific email as read both in the model and through the Gmail API
	/// </summary>
	/// <param name="email">Email object</param>
	/// <returns></returns>
	public async Task MarkEmailAsReadAsync(Email email)
	{
		if (email == null)
		{
			return;
		}

		Message msg = email.Message;

		// We remove the "UNREAD" label
		await ModifyEmailTagsAsync(msg, new List<string> { }, new List<string> { "UNREAD" });

		// We mark the model as read too
		email.IsRead = true;
	}

	/// <summary>
	/// Method for marking a specific email as unread both in the model and through the Gmail API
	/// </summary>
	/// <param name="email">Email object</param>
	/// <returns></returns>
	public async Task MarkEmailAsUnreadAsync(Email email)
	{
		if (email == null)
		{
			return;
		}

		Message msg = email.Message;

		// We add the "UNREAD" label
		await ModifyEmailTagsAsync(msg, new List<string> { "UNREAD" }, new List<string> { });

		// We mark the model as read too
		email.IsRead = false;
	}

	/// <summary>
	/// Method for marking a specific email as archived through the Gmail API
	/// </summary>
	/// <param name="email">Email object</param>
	/// <returns></returns>
	public async Task MarkEmailAsArchivedAsync(Email email)
	{
		if (email == null) 
		{ 
			return;
		}

		Message msg = email.Message;

		// We remove the "INBOX" label, this will archive the message
		await ModifyEmailTagsAsync(msg, new List<string> { }, new List<string> { "INBOX" });
	}

	/// <summary>
	/// Method for marking a specific email as trash through the Gmail API
	/// </summary>
	/// <param name="email">Email object</param>
	/// <returns></returns>
	public async Task MarkEmailAsTrashAsync(Email email)
	{
		if (email == null)
		{
			return;
		}

		Message msg = email.Message;

		// We remove the "INBOX" label and add the "TRASH" label
		await ModifyEmailTagsAsync(msg, new List<string> { "TRASH" }, new List<string> { "INBOX" });
	}

	/// <summary>
	/// Method for marking a specific email as favorite or "starred" through the Gmail API
	/// </summary>
	/// <param name="email">Email object</param>
	/// <returns></returns>
	public async Task MarkEmailAsFavoriteAsync(Email email)
	{
		if (email == null)
		{
			return;
		}

		Message msg = email.Message;

		// We add the "STARRED" label
		await ModifyEmailTagsAsync(msg, new List<string> { "STARRED" }, new List<string> { });

		email.IsFavorite = true;
	}

	/// <summary>
	/// Method for removing the tag of "starred" or favorite from a specific email through the Gmail API
	/// </summary>
	/// <param name="email">Email object</param>
	/// <returns></returns>
	public async Task MarkEmailAsNotFavoriteAsync(Email email)
	{
		if (email == null)
		{
			return;
		}

		Message msg = email.Message;

		// We add the "STARRED" label
		await ModifyEmailTagsAsync(msg, new List<string> { }, new List<string> { "STARRED" });

		email.IsFavorite = false;
	}

	/// <summary>
	/// Method for adding or removing specified labels to a given email
	/// </summary>
	/// <param name="email">Message object</param>
	/// <param name="addLabels">Labels that will be added</param>
	/// <param name="removeLabels">Labels that will be subtracted</param>
	/// <returns></returns>
	private async Task ModifyEmailTagsAsync(Message email, List<string> addLabels, List<string> removeLabels)
	{
		/*
		 * We create a messageRequest in which we specify the labels we'll add and substract from the Message
		 */
		ModifyMessageRequest messageRequest = new()
		{
			AddLabelIds = addLabels,
			RemoveLabelIds = removeLabels
		};

		GmailService service = await GetGmailServiceAsync();

		await service.Users.Messages.Modify(messageRequest, Me, email.Id).ExecuteAsync();
	}
	#endregion

	#region Sending + Drafting
	/// <summary>
	/// Method for sending an email through the Gmail API with the logged in user's email as the sender
	/// </summary>
	/// <see cref="CreateMessageAsync(Email)"/>
	/// <seealso cref="SaveDraftAsync(Email)"/>
	/// <param name="email">Email to be sent, the necessary properties are "To", "SubjectLine" and "Body"</param>
	/// <returns></returns>
	public async Task SendEmailAsync(Email email)
	{
		if (email == null)
		{
			return;
		}

		GmailService service = await GetGmailServiceAsync();

		// We create the Message object
		var message = await CreateMessageAsync(email);

		// We create and send the request
		var sendRequest = service.Users.Messages.Send(message, Me);
		await sendRequest.ExecuteAsync();
	}

	/// <summary>
	/// Method for sending a given draft through the Gmail API with the logged in user's email as the sender
	/// </summary>
	/// <see cref="Email"/>
	/// <seealso cref="SendEmailAsync(Email)"/>
	/// <param name="email">Email object</param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException">If the given email's DraftId is null or empty</exception>
	public async Task SendDraftAsync(Email email)
	{
        if (string.IsNullOrWhiteSpace(email.DraftId))
        {
			throw new ArgumentNullException("Email does not have a draft ID");
        }

		GmailService service = await GetGmailServiceAsync();

		// We create the Message object
		var message = await CreateMessageAsync(email);
		
		// We create the Draft object
		var draft = new Draft()
		{
			Id = email.DraftId,
			Message = message,
		};

		// We create and send the request
		var sendRequest = service.Users.Drafts.Send(draft, Me);
		await sendRequest.ExecuteAsync();
	}

	/// <summary>
	/// Method for creating a draft through the Gmail API with the logged in user's email as the sender
	/// </summary>
	/// <see cref="Email"/>
	/// <seealso cref="CreateMessageAsync(Email)"/>
	/// <seealso cref="SendEmailAsync(Email)"/>
	/// <param name="email">Email to be sent, the necessary properties are "To", "SubjectLine" and "Body"</param>
	/// <returns></returns>
	public async Task SaveDraftAsync(Email email)
	{
		if (email == null)
		{
			return;
		}

		GmailService service = await GetGmailServiceAsync();

		// We create the Message object
		var message = await CreateMessageAsync(email);

		// We create the draft object
		var draft = new Draft()
		{
			Message = message,
		};

		// We create and send the request
		var draftRequest = service.Users.Drafts.Create(draft, Me);
		await draftRequest.ExecuteAsync();
	}

	/// <summary>
	/// Method for deleting the desired draft using the DraftId from the email
	/// </summary>
	/// <see cref="Email"/>
	/// <param name="email"></param>
	/// <returns></returns>
	/// <exception cref="ArgumentNullException"></exception>
	public async Task DeleteDraftAsync(Email email)
	{
		if (string.IsNullOrWhiteSpace(email.DraftId))
		{
			throw new ArgumentNullException("Email does not have a draft ID");
		}

		GmailService service = await GetGmailServiceAsync();

		var request = service.Users.Drafts.Delete(Me, email.DraftId);
		await request.ExecuteAsync();
	}

	/// <summary>
	/// Method for creating a Message object based on a given Email object
	/// </summary>
	/// <param name="email">Email object to be used</param>
	/// <returns>Message object</returns>
	/// <exception cref="ArgumentNullException">If the email is null</exception>
	private async Task<Message> CreateMessageAsync(Email email)
	{
		if (email == null)
		{
			throw new ArgumentNullException(nameof(email));
		}

		/*
		 * To send or create drafts through the Gmail API we need to provide a Message object with the B64 encoded
		 * email (MIME) as the "Raw" property
		 */

		var oauth = await _oAuthService.GetOauthServiceAsync();
		var userInfo = await oauth.Userinfo.Get().ExecuteAsync();

		// We create and encode the MIME message
		var encodedEmail = Utilities.ConstructEmail(email.To, userInfo.Email, email.SubjectLine, email.Body);

		// We create the Message object
		var message = new Message
		{
			Raw = encodedEmail
		};

		return message;
	}
	#endregion
}
