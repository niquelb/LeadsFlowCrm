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
		var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
		return Convert.ToBase64String(plainTextBytes);
	}

	/// <summary>
	/// Method for decoding a given Base 64 string to plain text
	/// </summary>
	/// <param name="base64EncodedData">Encoded string</param>
	/// <returns>Plain text string</returns>
	public static string Base64Decode(string base64EncodedData)
	{
		var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
		return Encoding.UTF8.GetString(base64EncodedBytes);
	}
}