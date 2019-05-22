using System;

public class MobileSystemDialogButton
{
	public string ButtonLabel { get; }
	private readonly Action _action;

	public MobileSystemDialogButton(string buttonLabel, Action action = null)
	{
		ButtonLabel = buttonLabel;
		_action = action;
	}

	public void Action()
	{
		if (_action != null)
		{
			_action();
		}
	}
}
