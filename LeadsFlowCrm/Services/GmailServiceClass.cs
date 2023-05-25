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

namespace LeadsFlowCrm.Services;

public class GmailServiceClass : IGmailServiceClass
{
	private readonly IOAuthServiceClass _oAuthService;
	private readonly IBaseGoogleServiceClass _baseGoogleService;

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
		string userId = await GetUserEmailAsync();

		// We define the request to retrieve email list
		var emailListRequest = gmailService.Users.Messages.List(userId);

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
			output.Add(ProcessEmail(email, gmailService, userId));
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
	private static Email ProcessEmail(Message email, GmailService gmailService, string userId)
	{
		Email output = new();

		/*
		 * Retrieve the email details, this will create another "Message" object with the whole email with all the details
		 */
		var emailDetailsRequest = gmailService.Users.Messages.Get(userId, email.Id);
		emailDetailsRequest.Format = UsersResource.MessagesResource.GetRequest.FormatEnum.Full;

		Message emailContent = emailDetailsRequest.Execute();

		output.Snippet = emailContent.Snippet;

		/*
		 * Read the headers
		 */
		foreach (var header in emailContent.Payload.Headers)
		{
			switch (header.Name)
			{
				case "From":
					output.From = header.Value;
					break;
				case "Date":
					output.Date = Convert.ToDateTime(header.Value);
					break;
				case "Subject":
					output.SubjectLine = header.Value;
					break;
				default:
					break;
			}
		}

		/*
		 * Read the body
		 */
		if (emailContent.Payload.Parts == null && emailContent.Payload.Body != null)
		{
			output.EncodedBody = emailContent.Payload.Body.Data;
		}
		else
		{
			// TODO: deal with threads
		}

		output.Body = Utilities.Base64Decode(output.EncodedBody);

		// TODO: Get the attachments

		return output;
	}
}
