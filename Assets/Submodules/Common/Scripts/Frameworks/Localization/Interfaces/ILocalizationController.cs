using System;
using UnityEngine;

namespace Localization
{
    /// <summary>
    /// Basic localization interface for 
    /// </summary>
	public interface ILocalizationController
	{
		event Action<string> OnLocalizationChange;
		
		string CurrentLanguageCode { get; }

		void LocalizationChange(string languageCode);
		
		string GetText(string key);

		bool HasText(string key);
		
		GameObject GetGameObject(string key);
	}
}