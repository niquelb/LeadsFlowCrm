using Google.Apis.Gmail.v1.Data;
using System;

namespace LeadsFlowCrm.Models;

/// <summary>
/// Model class for the emails retrieved from the mailing provider
/// </summary>
public class Email
{
    /// <summary>
    /// Message object from the Gmail API
    /// </summary>
    public Message Message { get; set; } = new Message();

    /// <summary>
    /// Sender
    /// </summary>
    public string From { get; set; } = string.Empty;

    /// <summary>
    /// Sender's email address
    /// </summary>
    public string FromEmail { get; set; } = string.Empty;

	/// <summary>
	/// Reciever (used for sending or drafting emails)
	/// </summary>
	public string To { get; set; } = string.Empty;

    /// <summary>
    /// Subject line
    /// </summary>
    public string SubjectLine { get; set; } = string.Empty;

    /// <summary>
    /// Portion of the email's body
    /// </summary>
    public string Snippet { get; set; } = string.Empty;

    /// <summary>
    /// Email's body
    /// </summary>
    public string Body { get; set; } = string.Empty;

    /// <summary>
    /// Email's body encoded in Base64
    /// </summary>
    public string EncodedBody { get; set; } = string.Empty;

    /// <summary>
    /// Internal (and more accurate) date of the email
    /// </summary>
    public DateTime InternalDate { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Email date
    /// </summary>
    public DateTime Date { get; set; } = DateTime.MinValue;

    /// <summary>
    /// Is favorite
    /// </summary>
	public bool IsFavorite { get; set; }

    /// <summary>
    /// Is read
    /// </summary>
    public bool IsRead { get; set; }
}
