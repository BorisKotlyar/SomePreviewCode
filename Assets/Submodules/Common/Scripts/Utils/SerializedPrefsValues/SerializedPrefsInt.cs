using UnityEngine;
using System.Collections;

public sealed class SerializedPrefsInt : SerializedPrefsValue<int>
{
	public SerializedPrefsInt(string prefsName, int defaultValue) : base(prefsName, defaultValue) { }

	protected override int GetValue()
	{
		return PlayerPrefs.GetInt(_prefsName, _defaultValue);
	}

	protected override void SetValue(int value)
	{
		PlayerPrefs.SetInt(_prefsName, value);
	}
}
