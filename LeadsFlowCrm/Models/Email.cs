namespace LeadsFlowCrm.Models;

/// <summary>
/// Model class for the emails retrieved from the mailing provider
/// </summary>
public class Email
{
    public string Sender { get; set; } = string.Empty;
    public string SubjectLine { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsFavorite { get; set; }
    public bool IsRead { get; set; }
}
