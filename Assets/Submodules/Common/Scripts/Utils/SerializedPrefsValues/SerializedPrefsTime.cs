using UnityEngine;
using System.Collections;
using System.Globalization;

public class SerializedPrefsTime : SerializedPrefsValue<System.DateTime>
{
	public SerializedPrefsTime(string prefsName, System.DateTime defaultValue) : base(prefsName, defaultValue) { }

	private readonly string _timeFormatter = "yyyy.MM.dd hh.mm.ss tt zzz";
	CultureInfo _provider = CultureInfo.CurrentCulture;

	protected override System.DateTime GetValue()
	{
		return System.DateTime.ParseExact (PlayerPrefs.GetString (_prefsName, _defaultValue.ToString (_timeFormatter, _provider)), _timeFormatter, _provider);
	}

	protected override void SetValue(System.DateTime value)
	{
		PlayerPrefs.SetString (_prefsName, value.ToString (_timeFormatter, _provider));
	}
}
