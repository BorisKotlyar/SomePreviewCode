using System.Collections.Generic;
using Common.Utils;

namespace Localization.Data
{
	public class LocalizationLanguagesSettings
	{
		private readonly LocalizationLanguagesSettingsData _languagesSettingsData;

		public string DefaultLanguageCode
		{
			get { return _languagesSettingsData.DefaultLanguageCode; }
			#if UNITY_EDITOR
			set
			{
				if (string.Equals(_languagesSettingsData.DefaultLanguageCode, value)) return;
				
				_languagesSettingsData.DefaultLanguageCode = value;
				Save(_languagesSettingsData);
			}
			#endif
		}
		public bool AutoDetectLanguage
		{
			get { return _languagesSettingsData.AutoDetectLanguage; }
			#if UNITY_EDITOR
			set
			{
				if (_languagesSettingsData.AutoDetectLanguage == value) return;
				
				_languagesSettingsData.AutoDetectLanguage = value;
				Save(_languagesSettingsData);
			}
			#endif
		}
		public List<string> SupportedLanguages
		{
			get { return _languagesSettingsData.SupportedLanguages; }
			#if UNITY_EDITOR
			set
			{
				_languagesSettingsData.SupportedLanguages = value;
				Save(_languagesSettingsData);
			}
			#endif
		}
		
		public LocalizationLanguagesSettings()
		{
			_languagesSettingsData = Load();
		}
		
		#if UNITY_EDITOR
		private static void Save(LocalizationLanguagesSettingsData settingsData)
		{
			FileManager.SaveToResources(LocalizationConsts.LanguagesSettingsFile, settingsData);
		}
		#endif

		private static LocalizationLanguagesSettingsData Load()
		{
			var settingsData = FileManager.LoadFromResources<LocalizationLanguagesSettingsData>(
				LocalizationConsts.LanguagesSettingsFile);
            
			if (settingsData != null) return settingsData;

			settingsData = new LocalizationLanguagesSettingsData {AutoDetectLanguage = true};
			#if UNITY_EDITOR
			Save(settingsData);
			#endif
			return settingsData;
		}
	}
}
