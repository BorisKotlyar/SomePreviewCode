using Common.Utils;
using UnityEngine;
using Localization.Data;
using Zenject;
using System.Collections.Generic;

namespace Localization
{
	public class GoogleSheetsLocalizationController : ILocalizationController
	{
		[Inject] private readonly IDebugService _debugService;
		
		private readonly LocalizationLanguagesSettings _languagesSettings = new LocalizationLanguagesSettings();

		public event System.Action<string> OnLocalizationChange = languageCode => { };

        private readonly Dictionary<string, string> _languageOverrides = new Dictionary<string, string>()
        {
            { "ru-ru", "ru"},
            { "ru_ru", "ru"},
            { "ru_ua", "ru"},
            { "ru_be", "ru"},
            { "en_us", "en"},
            { "fr-ca", "en"},
            { "zh-hk", "zh-tw"},
            { "be", "ru"},
            { "uk", "ru"}
        };

        private readonly Dictionary<SystemLanguage, string> _systemLanguageCodes = new Dictionary<SystemLanguage, string>()
        {
            { SystemLanguage.Russian, "ru"},
            { SystemLanguage.Ukrainian, "ru"},
            { SystemLanguage.Belarusian, "ru"},
            { SystemLanguage.French, "fr"},
            { SystemLanguage.English, "en"},
            { SystemLanguage.German, "de"},
            { SystemLanguage.Spanish, "sp"},
            { SystemLanguage.Portuguese, "pt"},
        };

        private string SavedLanguageCode
		{
			get
			{
				return PlayerPrefs.GetString(LocalizationConsts.SavedLanguageCodePrefs, GetLanguageCode());
			}
			set { PlayerPrefs.SetString(LocalizationConsts.SavedLanguageCodePrefs, value); }
		}

		public LocalizationLanguage CurrentLanguage { get; private set; }

		public string CurrentLanguageCode
		{
			get { return CurrentLanguage.LanguageCode; }
		}

		public GoogleSheetsLocalizationController()
		{
#if UNITY_EDITOR
            Debug.Log("[GoogleSheetsLocalizationController] AutoDetectLanguage: " + _languagesSettings.AutoDetectLanguage);
#endif
            var languageCode = _languagesSettings.AutoDetectLanguage ? GetLanguageCode() : SavedLanguageCode;
			LocalizationChange(languageCode);
		}

		public virtual void LocalizationChange(string languageCode)
		{
#if UNITY_EDITOR
            Debug.Log("[LocalizationChange] languageCode: " + languageCode);
            Debug.Log("[LocalizationChange] System language: " + Application.systemLanguage);
#endif
            if (!_languagesSettings.SupportedLanguages.Contains(languageCode)) return;

            CurrentLanguage = FileManager.LoadFromResources<LocalizationLanguage>(
				string.Format(LocalizationConsts.TextLocalizationFolder, string.Format("{0}.json", languageCode)));
			SavedLanguageCode = languageCode;

			OnLocalizationChange.Invoke(languageCode);

            Debug.Log("[LocalizationChange] success, languageCode: " + languageCode);
        }

		public string GetText(string key)
		{
			var value = string.Empty;
			if (!CurrentLanguage.TextValues.TryGetValue(key, out value))
			{
				_debugService.Log(string.Format("Missing text with key: {0}", key), Color.red);
			}

			if (string.IsNullOrEmpty(value))
			{
				_debugService.Log(
					string.Format("Empty localized text with key: {0}, languageCode: {1}", key, CurrentLanguageCode),
					Color.red);
			}

			return value;
		}

		public bool HasText(string key)
		{
			var value = string.Empty;
			return CurrentLanguage.TextValues.TryGetValue(key, out value);
		}

		public GameObject GetGameObject(string key)
		{
			var prefab =
				Resources.Load(string.Format("Localization/Prefabs/{0}/{1}", CurrentLanguageCode, key)) as GameObject;
			if (prefab == null)
			{
				Debug.LogError(string.Format("Missing localized prefab with key: {0}, languageCode: {1}", key,
					CurrentLanguageCode));
			}

			return prefab;
		}
		
		protected virtual string GetLanguageCode()
		{
#if UNITY_IOS
            if (_systemLanguageCodes.ContainsKey(Application.systemLanguage))
            {
                var sysLang = _systemLanguageCodes[Application.systemLanguage];

                if (_languagesSettings.SupportedLanguages.Contains(sysLang))
                {
                    return sysLang;
                }
            }
#endif
            string code;
			return GetLanguageWithDialect(out code) ? code : GetLanguage();
		}

		protected virtual bool GetLanguageWithDialect(out string code)
		{
			var languageId = PreciseLocale.GetLanguageID().ToLower();

            Debug.Log("[GetLanguageWithDialect] languageId: " + languageId);

            if (!_languagesSettings.SupportedLanguages.Contains(languageId))
            {
                if (_languageOverrides.ContainsKey(languageId))
                {
                    var overrideLang = _languageOverrides[languageId];

                    if (_languagesSettings.SupportedLanguages.Contains(overrideLang))
                    {
                        code = overrideLang;
                        return true;
                    }
                }

                code = string.Empty;
                return false;
            }

			code = languageId;
			return true;
		}

		protected virtual string GetLanguage()
		{
			var language = PreciseLocale.GetLanguage().ToLower();

            Debug.Log("[GetLanguage] language: " + language);

            if (!_languagesSettings.SupportedLanguages.Contains(language))
            {
                if (_languageOverrides.ContainsKey(language))
                {
                    var overrideLang = _languageOverrides[language];

                    if (_languagesSettings.SupportedLanguages.Contains(overrideLang))                    
                        return overrideLang;                    
                }

                return _languagesSettings.DefaultLanguageCode;
            }

			return language;
		}
	}
}
