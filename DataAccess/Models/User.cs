using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Models;

public class User
{
    public String Id { get; set; } = Guid.NewGuid().ToString();
    public String OAuthToken { get; set; }
    public String Email { get; set; }
    public String DisplayName { get; set; }
    public String OrganizationId { get; set; }

}
