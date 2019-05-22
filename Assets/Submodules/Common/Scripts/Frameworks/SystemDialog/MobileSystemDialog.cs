using System;
using System.Collections.Generic;

public class MobileSystemDialog
{
	public string TitleText { get; }
	public string BodyText { get; }
	public List<MobileSystemDialogButton> Buttons { get; }
	public Action OnDissmiss { get; }

	public MobileSystemDialog(string titleText, string bodyText,
		List<MobileSystemDialogButton> buttons, Action onDissmiss = null)
	{
		TitleText = titleText;
		BodyText = bodyText;
		Buttons = buttons;
		OnDissmiss = onDissmiss;
	}
}
