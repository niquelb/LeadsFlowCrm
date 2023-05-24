using System;

namespace LeadsFlowCrm.Models;

/// <summary>
/// Model class for the emails retrieved from the mailing provider
/// </summary>
public class Email
{
    /// <summary>
    /// Sender
    /// </summary>
    public string From { get; set; }

    /// <summary>
    /// Reciever
    /// </summary>
    public string To { get; set; }

    /// <summary>
    /// Subject line
    /// </summary>
    public string SubjectLine { get; set; }

    /// <summary>
    /// Portion of the email's body
    /// </summary>
    public string Snippet { get; set; }

    /// <summary>
    /// Email's body
    /// </summary>
    public string Body { get; set; }

    /// <summary>
    /// Email's body encoded in Base64
    /// </summary>
    public string EncodedBody { get; set; }

    /// <summary>
    /// Email date
    /// </summary>
    public DateTime Date { get; set; }

    public bool IsFavorite { get; set; }
    public bool IsRead { get; set; }
}
