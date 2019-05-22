using UnityEngine;
using System.Collections;

public sealed class SerializedPrefsBool : SerializedPrefsValue<bool>
{
	public SerializedPrefsBool(string prefsName, bool defaultValue) : base(prefsName, defaultValue) { }

	protected override bool GetValue()
	{
		return (PlayerPrefs.GetInt(_prefsName, (_defaultValue) ? 1 : 0)==0) ? false : true;
	}

	protected override void SetValue(bool value)
	{
		PlayerPrefs.SetInt(_prefsName, (value) ? 1 : 0);
	}
}
