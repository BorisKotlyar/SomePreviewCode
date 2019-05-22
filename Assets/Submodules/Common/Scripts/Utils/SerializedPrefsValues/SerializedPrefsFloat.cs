using UnityEngine;
using System.Collections;

public sealed class SerializedPrefsFloat : SerializedPrefsValue<float>
{
	public SerializedPrefsFloat(string prefsName, float defaultValue) : base(prefsName, defaultValue) { }

	protected override float GetValue()
	{
		return PlayerPrefs.GetFloat(_prefsName, _defaultValue);
	}

	protected override void SetValue(float value)
	{
		PlayerPrefs.SetFloat(_prefsName, value);
	}
}
