using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.EventModels;

/// <summary>
/// This is an event to create a draft with given contacts as recipients
/// </summary>
public class DraftEvent
{
    /// <summary>
    /// Email addresses that will be added as recipients
    /// </summary>
    public List<string> Recipients { get; set; } = new();
}
