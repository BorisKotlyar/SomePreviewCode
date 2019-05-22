using UnityEngine;
using System.Collections;

public sealed class SerializedPrefsString : SerializedPrefsValue<string>
{
	public SerializedPrefsString(string prefsName, string defaultValue) : base(prefsName, defaultValue) { }

	protected override string GetValue()
	{
		return PlayerPrefs.GetString(_prefsName, _defaultValue);
	}

	protected override void SetValue(string value)
	{
		PlayerPrefs.SetString(_prefsName, value);
	}
}
