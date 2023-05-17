namespace LeadsFlowCrm.Models;

/// <summary>
/// Model class for the emails retrieved from the mailing provider
/// </summary>
public class EmailModel
{
    public string Sender { get; set; }
    public string SubjectLine { get; set; }
    public string Body { get; set; }
    public bool IsFavorite { get; set; }
    public bool IsRead { get; set; }
}
