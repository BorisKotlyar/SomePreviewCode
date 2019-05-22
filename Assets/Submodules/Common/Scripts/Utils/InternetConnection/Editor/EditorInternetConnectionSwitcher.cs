using UnityEditor;
using UnityEngine;

public static class EditorInternetConnectionSwitcher
{
	private const string InternetExistsCheckerPrefs = "EDITOR_INTERNET_EXISTS_CHECKER";
	
	public static bool InternetExistsCheckerValue
	{
		get { return PlayerPrefs.GetInt(InternetExistsCheckerPrefs, 0) == 1; }
		set { PlayerPrefs.SetInt(InternetExistsCheckerPrefs, value ? 1 : 0); }
	}
	
	[MenuItem("DevGame/Editor/Internet/Forced off", true)]
	private static bool ShowDetermineStopOnRecompile()
	{
		return !InternetExistsCheckerValue;
	}

	[MenuItem("DevGame/Editor/Internet/Forced off")]
	private static void DetermineStopOnRecompile()
	{
		InternetExistsCheckerValue = true;
	}
	
	[MenuItem("DevGame/Editor/Internet/Autodetect", true)]
	private static bool ShowDetermineDontStopOnRecompile()
	{
		return InternetExistsCheckerValue;
	}

	[MenuItem("DevGame/Editor/Internet/Autodetect")]
	private static void DetermineDontStopOnRecompile()
	{
		InternetExistsCheckerValue = false;
	}
}
