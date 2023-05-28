using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Utils;

/// <summary>
/// Utility class
/// </summary>
public static class Utilities
{
	/// <summary>
	/// Method for decoding a Base 64 string with special characters from the Gmail API to plain text
	/// </summary>
	/// <param name="base64EncodedData">Encoded string</param>
	/// <returns>Plain text string</returns>
	public static string Base64Decode(string base64EncodedData)
	{
		// We first remove illegal characters
		StringBuilder sb = new(base64EncodedData);
		sb.Replace("-", "+");
		sb.Replace("_", "/");
		sb.Replace(" ", "+");
		sb.Replace("=", "+");


		base64EncodedData = sb.ToString();

		// We convert it and return it
		var output = Convert.FromBase64String(base64EncodedData);
		return Encoding.UTF8.GetString(output);
	}

	/// <summary>
	/// Method that creates a MIME message that then gets encoded in Base 64 for use with the Gmail API
	/// </summary>
	/// <param name="to">Reciever</param>
	/// <param name="from">Sender</param>
	/// <param name="subject">Subject line</param>
	/// <param name="body">Email body</param>
	/// <returns>Encoded email</returns>
	/// <seealso cref="MimeMessage"/>
	public static string ConstructEmail(string to, string from, string subject, string body)
	{
		// We create the MIME message using MimeKit
		var message = new MimeMessage();
		message.From.Add(new MailboxAddress("", from));
		message.To.Add(new MailboxAddress("", to));
		message.Subject = subject;

		BodyBuilder bodyBuilder = new()
		{
			TextBody = body
		};

		message.Body = bodyBuilder.ToMessageBody();

		/*
		 * We then write the message to a MemoryStream, this is done so we can
		 * then cast that stream to an array of bytes and decode it to Base 64
		 */
		using var memoryStream = new MemoryStream();

		message.WriteTo(memoryStream);
		return Convert.ToBase64String(memoryStream.ToArray());
	}
}