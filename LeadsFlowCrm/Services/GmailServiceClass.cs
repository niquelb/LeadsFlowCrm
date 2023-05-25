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

namespace LeadsFlowCrm.Services;

public class GmailServiceClass : IGmailServiceClass
{
	private readonly IOAuthServiceClass _oAuthService;
	private readonly IBaseGoogleServiceClass _baseGoogleService;
	/// <summary> Special keyword reserved for referencing the logged in user </summary>
	private const string me = "me";

	/// <summary> Service object for the Gmail API </summary>
	private GmailService _gmailService;

	public GmailServiceClass(IBaseGoogleServiceClass baseGoogleService, IOAuthServiceClass oAuthService)
	{
		_baseGoogleService = baseGoogleService;
		_oAuthService = oAuthService;
	}

	/// <summary>
	/// Method for retrieving the logged in user's email through the OAuth service
	/// </summary>
	/// <returns>Logged in user's email</returns>
	private async Task<string> GetUserEmailAsync()
	{
		// We retrieve the OAuth service
		var oauthService = await _oAuthService.GetOauthServiceAsync();

		// We retrieve the user's profile information
		var userInfo = await oauthService.Userinfo.Get().ExecuteAsync();

		// We collect the user's info
		string email = userInfo.Email;

		return email;
	}

	/// <summary>
	/// Method for retrieving the Gmail service object for the Gmail API
	/// </summary>
	/// <returns>GmailService object</returns>
	public async Task<GmailService> GetGmailServiceAsync()
	{
		if (_gmailService == null)
		{
			_gmailService = new GmailService(await _baseGoogleService.GetServiceAsync());
		}

		return _gmailService;
	}

	public async Task<List<Email>> GetEmailsFromInboxAsync()
	{
		List<Email> output = new();

		// We get the service and the user's email
		GmailService gmailService = await GetGmailServiceAsync();

		// We define the request to retrieve email list
		var emailListRequest = gmailService.Users.Messages.List(me);

		// Only get emails labeled as 'INBOX':
		emailListRequest.LabelIds = new[] { "INBOX" };

		// Not include spam/trash
		emailListRequest.IncludeSpamTrash = false;

		// Execute the request to retrieve the list of emails
		ListMessagesResponse emailListResponse = emailListRequest.Execute();

		// If there are no emails in the inbox or there was an error
		if (emailListResponse == null || emailListResponse.Messages == null)
		{
			throw new Exception("Error parsing the emails");
		}

		foreach (Message email in emailListResponse.Messages)
		{
			output.Add(ProcessEmail(email, gmailService));
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
	private static Email ProcessEmail(Message email, GmailService gmailService)
	{
		Email output = new();

		/*
		 * Retrieve the email details, this will create another "Message" object with the whole email with all the details
		 */
		var request = gmailService.Users.Messages.Get(me, email.Id);
		request.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;
		request.MetadataHeaders = new string[] { "labelIds" };

		Message emailContent = request.Execute();

		/*
		 * We associate this email with the Email model
		 */
		output.Message = emailContent;

		/*
		 * If the email is "starred" we mark it as favorite
		 */
		output.IsFavorite = emailContent.LabelIds.Contains("STARRED");

		/*
		 * We check if the email is read or unread
		 */
		output.IsRead = !(emailContent.LabelIds.Contains("UNREAD"));

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
					string[] format = { "ddd, dd MMM yyyy HH:mm:ss zzz '('zzz')'" };
					try
					{
						DateTimeOffset dtf = DateTimeOffset.ParseExact(dateString, format, CultureInfo.InvariantCulture);
						output.Date = dtf.DateTime;
					}
					catch (Exception) {}
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

		return output;
	}
}
