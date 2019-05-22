using UnityEngine;
using System.Collections;

public abstract class SerializedPrefsValue<T>
{
	public SerializedPrefsValue(string prefsName, T defaultValue)
	{
		_prefsName = prefsName;
		_defaultValue = defaultValue;
	}

	protected string _prefsName;
	protected T _defaultValue;

	public T Value
	{
		get { return GetValue(); }
		set { SetValue(value); }
	}

	protected abstract T GetValue();

	protected abstract void SetValue(T value);
}
