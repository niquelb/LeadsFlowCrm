using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeadsFlowCrm.Utils;

/// <summary>
/// Utility class
/// </summary>
public static class Utilities
{
	/// <summary>
	/// Method for encoding a given string to Base 64
	/// </summary>
	/// <param name="plainText">Plain text string</param>
	/// <returns>Encoded string</returns>
	public static string Base64Encode(string plainText)
	{
		var output = Encoding.UTF8.GetBytes(plainText);
		return Convert.ToBase64String(output);
	}

	/// <summary>
	/// Method for decoding a given Base 64 string to plain text
	/// </summary>
	/// <param name="base64EncodedData">Encoded string</param>
	/// <returns>Plain text string</returns>
	public static string Base64Decode(string base64EncodedData)
	{
		// We first remove illegal characters
		StringBuilder sb = new StringBuilder();
		sb.Append(base64EncodedData.Replace("-", "+"));
		sb.Append(base64EncodedData.Replace("_", "/"));
		sb.Append(base64EncodedData.Replace(" ", "+"));
		sb.Append(base64EncodedData.Replace("=", "+"));

		base64EncodedData = sb.ToString();

		// We standardize the lenght
		if (base64EncodedData.Length % 4 > 0) 
		{
			base64EncodedData += new string('=', 4 - base64EncodedData.Length % 4); 
		}
		else if (base64EncodedData.Length % 4 == 0)
		{
			base64EncodedData = base64EncodedData.Substring(0, base64EncodedData.Length - 1);
			if (base64EncodedData.Length % 4 > 0) { base64EncodedData += new string('+', 4 - base64EncodedData.Length % 4); }
		}

		// We convert it and return it
		var output = Convert.FromBase64String(base64EncodedData);
		return Encoding.UTF8.GetString(output);
	}
}