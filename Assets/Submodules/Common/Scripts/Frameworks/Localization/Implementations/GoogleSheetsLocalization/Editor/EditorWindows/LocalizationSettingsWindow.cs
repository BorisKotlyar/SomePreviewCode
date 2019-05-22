using Localization.Data;
using UnityEditor;
using UnityEngine;

namespace Localization.Editor
{
	public class LocalizationSettingsWindow : EditorWindow
	{
		private const float LabelsWidth = 220.0f;
		private const float ValuesWidth = 300.0f;
		
		private LocalizationLanguagesSettings _languagesSettings;

		private void OnEnable()
		{
			LocalizationImporter.OnLocalizationImported += LocalizationImportedHandler;
		}
		
		private void OnDisable()
		{
			LocalizationImporter.OnLocalizationImported -= LocalizationImportedHandler;
		}

		private void LocalizationImportedHandler()
		{
			_languagesSettings = new LocalizationLanguagesSettings();
		}

		private void OnGUI()
		{
			if (_languagesSettings == null)
			{
				_languagesSettings = new LocalizationLanguagesSettings();
			}

			GUILayout.BeginVertical();

			ConnectionSettingsDraw();
			EditorGUILayout.Separator();
			LanguagesSettingsDraw();

			GUILayout.EndVertical();
		}
		
		private void ConnectionSettingsDraw()
		{
			GUILayout.Label("    API settings", EditorStyles.boldLabel, GUILayout.Width(LabelsWidth));
			
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Spreadsheet API key:", GUILayout.Width(LabelsWidth));
			var spreadsheetApiKeyValue = EditorGUILayout.TextField(
				LocalizationConnectionSettings.SpreadsheetApiKey, GUILayout.Width(ValuesWidth));
			if (!string.Equals(spreadsheetApiKeyValue, LocalizationConnectionSettings.SpreadsheetApiKey))
			{
				LocalizationConnectionSettings.SpreadsheetApiKey = spreadsheetApiKeyValue;
			}
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Spreadsheet sheet name:", GUILayout.Width(LabelsWidth));
			var spreadsheetSheetNameValue = EditorGUILayout.TextField(
				LocalizationConnectionSettings.SpreadsheetSheetName, GUILayout.Width(ValuesWidth));
			if (!string.Equals(spreadsheetSheetNameValue, LocalizationConnectionSettings.SpreadsheetSheetName))
			{
				LocalizationConnectionSettings.SpreadsheetSheetName = spreadsheetSheetNameValue;
			}

			GUILayout.EndHorizontal();

			if (string.IsNullOrEmpty(spreadsheetApiKeyValue)) return;
			
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("", GUILayout.Width(LabelsWidth));
			if(GUILayout.Button("Open google document", GUILayout.Width(ValuesWidth)))
			{
				Application.OpenURL(string.Format(LocalizationConsts.GoogleDocUrl, spreadsheetApiKeyValue));
			}
			GUILayout.EndHorizontal();
		}

		private void LanguagesSettingsDraw()
		{
			GUILayout.Label("    Languages settings", EditorStyles.boldLabel, GUILayout.Width(LabelsWidth));
			
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Default language:", GUILayout.Width(LabelsWidth));
			if(!_languagesSettings.SupportedLanguages.IsNullOrEmpty())
			{
				var selectedLanguageValue =
					Mathf.Clamp(
						_languagesSettings.SupportedLanguages.FindIndex(
							a => a == _languagesSettings.DefaultLanguageCode), 0,
						_languagesSettings.SupportedLanguages.Count - 1);
				selectedLanguageValue = EditorGUILayout.Popup(selectedLanguageValue,
					_languagesSettings.SupportedLanguages.ToArray(), GUILayout.Width(ValuesWidth));
				_languagesSettings.DefaultLanguageCode = _languagesSettings.SupportedLanguages[selectedLanguageValue];
			}
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("Auto-detect language every app start*:",
				GUILayout.Width(LabelsWidth));
			_languagesSettings.AutoDetectLanguage =
				EditorGUILayout.Toggle(_languagesSettings.AutoDetectLanguage, GUILayout.Width(ValuesWidth));
			GUILayout.EndHorizontal();

			EditorGUILayout.LabelField("");
			
			EditorGUILayout.LabelField(
				"*Рекомендуется использовать только в приложениях где нет кнопок переключения языков",
				GUILayout.Width(LabelsWidth + ValuesWidth));
		}
	}
}
