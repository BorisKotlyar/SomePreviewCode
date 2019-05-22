using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization;
using System.Globalization;
using System.Linq;
using Localization;
using Localization.Data;
using TMPro;
using Zenject;

namespace Localization
{
	public class SmartLocalizationController : ILocalizationController
	{
		[Inject] private readonly IDebugService _debugService;
		
		public event System.Action<string> OnLocalizationChange = languageCode => { };

		private string SavedLanguageCode
		{
			get { return PlayerPrefs.GetString(LocalizationConsts.SavedLanguageCodePrefs, string.Empty); }
			set { PlayerPrefs.SetString(LocalizationConsts.SavedLanguageCodePrefs, value); }
		}

		public string CurrentLanguageCode
		{
			get { return LanguageManager.Instance.CurrentlyLoadedCulture.languageCode; }
		}

		public SmartLocalizationController()
		{
			LanguageManager.SetDontDestroyOnLoad();
			LanguageManager.Instance.OnChangeLanguage += OnSmartLocalizationChangeLanguage;

			DetectLanguge();
		}

		public virtual void LocalizationChange(string languageCode)
		{
			if (LanguageManager.Instance.GetSupportedLanguages().All(obj => obj.languageCode != languageCode)) return;

			SavedLanguageCode = languageCode;
			DetectLanguge();
		}

		protected virtual void DetectLanguge()
		{
			var storedLang = SavedLanguageCode;
			if (string.IsNullOrEmpty(storedLang))
			{
				var supportedLang = LanguageManager.Instance.GetDeviceCultureIfSupported();
				if (supportedLang != null)
				{
					LanguageManager.Instance.ChangeLanguage(supportedLang);
					SavedLanguageCode = LanguageManager.Instance.CurrentlyLoadedCulture.languageCode;
				}
				else
				{
					if (Application.systemLanguage == SystemLanguage.Russian ||
					    Application.systemLanguage == SystemLanguage.Ukrainian ||
					    Application.systemLanguage == SystemLanguage.Belarusian)
					{
						if (LanguageManager.Instance.GetSupportedLanguages().Any(obj => obj.languageCode == "ru"))
						{
							LanguageManager.Instance.ChangeLanguage("ru");
							SavedLanguageCode = LanguageManager.Instance.CurrentlyLoadedCulture.languageCode;
						}
					}

					else if (Application.systemLanguage == SystemLanguage.ChineseTraditional ||
					         Application.systemLanguage == SystemLanguage.ChineseSimplified ||
					         Application.systemLanguage == SystemLanguage.Chinese)
					{
						if (LanguageManager.Instance.GetSupportedLanguages().Any(obj => obj.languageCode == "zh-CN"))
						{
							LanguageManager.Instance.ChangeLanguage("zh-CN");
							SavedLanguageCode = LanguageManager.Instance.CurrentlyLoadedCulture.languageCode;
						}
					}

					//_debugService.Log (DebugServiceTagConsts.SMLOC, "Supported Lang is null", Color.red);
				}
			}
			else
			{
				//_debugService.Log (DebugServiceTagConsts.SMLOC, string.Format("Load stored language code: {0}", storedLang), Color.green);
				LanguageManager.Instance.ChangeLanguage(storedLang);
			}
		}

		private void OnSmartLocalizationChangeLanguage(LanguageManager manager)
		{
			OnLocalizationChange.Invoke(CurrentLanguageCode);
		}

		public string GetText(string key)
		{
			return LanguageManager.Instance.GetTextValue(key);
		}

		public bool HasText(string key)
		{
			return LanguageManager.Instance.HasKey(key);
		}

		public GameObject GetGameObject(string key)
		{
			return LanguageManager.Instance.GetPrefab(key);
		}
	}
}