using System.Windows;

namespace LeadsFlowCrm.Utils;

/// <summary>
/// Class responsible for allowing a textbox to contain a "hint" text
/// </summary>
public static class TextBoxHelper
{
	public static readonly DependencyProperty HintProperty =
		DependencyProperty.RegisterAttached(
			"Hint",
			typeof(string),
			typeof(TextBoxHelper),
			new PropertyMetadata(null));

	public static string GetHint(DependencyObject obj)
	{
		return (string)obj.GetValue(HintProperty);
	}

	public static void SetHint(DependencyObject obj, string value)
	{
		obj.SetValue(HintProperty, value);
	}
}

