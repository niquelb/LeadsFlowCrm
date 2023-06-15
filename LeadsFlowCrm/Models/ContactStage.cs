using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Models;

/// <summary>
/// Model for associating a contact with a given stage
/// </summary>
/// <seealso cref="Contact"/>
/// <seealso cref="Stage"/>
public class ContactStage
{
    public Stage GivenStage { get; set; } = new();

    public string StageName { get; set; } = string.Empty;

    public bool IsCurrentStage { get; set; }
}
