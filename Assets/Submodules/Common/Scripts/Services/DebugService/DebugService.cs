using System;
using UnityEngine;

public class DebugService : IDebugService
{
	public event Action<string> OnLog = str => { };

	private bool IsEditor
	{
		get
		{
			return Application.platform == RuntimePlatform.WindowsEditor ||
			       Application.platform == RuntimePlatform.OSXEditor ||
			       Application.platform == RuntimePlatform.LinuxEditor;
		}
	}

	public void Initialize()
	{
		//create filestream to log into file
		//subscribe to log to catch exceptions
	}

	public void Dispose()
	{
		//close filestream
	}

	public void Log(string message)
	{
		if (!Debug.isDebugBuild) return;

		Debug.Log(message);
		OnLog.Invoke(message);
	}

	public void Log(string message, Color color)
	{
		if (IsEditor)
		{
			Log(GetMessageWithColor(color, message));
		}
		else
		{
			Log(message);
		}
	}

	public void Log(string tag, string message)
	{
		Log(GetMessageWithTag(tag, message));
	}

	public void Log(string tag, string message, Color color)
	{
		if (IsEditor)
		{
			Log(GetMessageWithTag(tag, message), color);
		}
		else
		{
			Log(tag, message);
		}
	}

	private string GetMessageWithTag(string tag, string message)
	{
		return string.Format("{0}: {1}", tag.ToUpper(), message);
	}

	private string GetMessageWithColor(Color color, string message)
	{
		return string.Format("<color=#{0}>{1}</color>", ColorUtility.ToHtmlStringRGB(color), message);
	}
}
