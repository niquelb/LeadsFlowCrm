using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using LeadsFlowCrm.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Services;

public class GmailServiceClass
{
	private readonly IOAuthServiceClass _oAuthService;

	/// <summary> Service object for the Gmail API </summary>
	private GmailService _gmailService;

    public GmailServiceClass(IOAuthServiceClass oAuthService)
    {
		_oAuthService = oAuthService;
	}

    /// <summary>
    /// Method for retrieving the Gmail service object for the Gmail API
    /// </summary>
    /// <returns>GmailService object</returns>
    public async Task<GmailService> GetGmailServiceAsync()
	{
		if (_gmailService == null)
		{
			_gmailService = new GmailService(new BaseClientService.Initializer()
			{
				HttpClientInitializer = await _oAuthService.GetCredentialsAsync(),
				ApplicationName = GlobalVariables.appName,
			});
		}

		return _gmailService;
	}
}
