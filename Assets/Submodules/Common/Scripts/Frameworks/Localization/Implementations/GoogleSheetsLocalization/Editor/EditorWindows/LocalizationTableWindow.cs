using System.Collections.Generic;
using System.Linq;
using System.IO;
using Common.Utils;
using Localization.Data;
using UnityEngine;
using UnityEditor;

namespace Localization.Editor
{
	public class LocalizationTableWindow : EditorWindow
	{
		private LocalizationLanguagesSettings _languagesSettings;
		private Vector2 _scrollPosition;
		private LocalizationLanguage _rootLanguage;
		private LocalizationLanguage _additionalLanguage;
		private int _additionalLanguageId;
		private string _keyFilter;
		private string _rootLanguageFilter;
		private string _additionalLanguageFilter;

		private void OnEnable()
		{
			LocalizationImporter.OnLocalizationImported += LocalizationImportedHandler;
			CheckData();
		}
		
		private void OnDisable()
		{
			LocalizationImporter.OnLocalizationImported -= LocalizationImportedHandler;
		}

		private void LocalizationImportedHandler()
		{
			_languagesSettings = new LocalizationLanguagesSettings();
			_rootLanguage = LoadLanguage("ru");
			_additionalLanguage = LoadLanguage(_languagesSettings.SupportedLanguages[_additionalLanguageId]);
		}
		
		private void CheckData()
		{
			if (_languagesSettings == null)
			{
				_languagesSettings = new LocalizationLanguagesSettings();
			}
			
			if (_rootLanguage == null)
			{
				_rootLanguage = LoadLanguage("ru");
			}
			if (_additionalLanguage == null)
			{
				_additionalLanguage = LoadLanguage(_languagesSettings.SupportedLanguages[_additionalLanguageId]);
			}
		}

		private void OnGUI()
		{
			GUILayout.BeginVertical();
			
			LocalizationTableDraw();
			
			GUILayout.EndVertical();
		}
		
		public void LocalizationTableDraw()
		{
			var width = position.width * 0.95f / 3.0f;

			GUILayout.BeginHorizontal();
			_keyFilter = EditorGUILayout.TextField(_keyFilter, GUILayout.Width(width));
			_rootLanguageFilter = EditorGUILayout.TextField(_rootLanguageFilter, GUILayout.Width(width));
			_additionalLanguageFilter = EditorGUILayout.TextField(_additionalLanguageFilter, GUILayout.Width(width));
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			EditorGUILayout.LabelField("key", GUILayout.Width(width));
			EditorGUILayout.LabelField("ru", GUILayout.Width(width));
			if (!_languagesSettings.SupportedLanguages.IsNullOrEmpty())
			{
				var cachedLanguageTableId = _additionalLanguageId;
				_additionalLanguageId = EditorGUILayout.Popup(_additionalLanguageId,
					_languagesSettings.SupportedLanguages.ToArray(), GUILayout.Width(width));

				if (cachedLanguageTableId != _additionalLanguageId)
				{
					_additionalLanguage = LoadLanguage(_languagesSettings.SupportedLanguages[_additionalLanguageId]);
				}
			}

			GUILayout.EndHorizontal();
			EditorGUILayout.Separator();

			_scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

			var totalHeight = 0.0f;

			foreach (var kvp in GetFilteredValues(_rootLanguage.TextValues,
				_additionalLanguage == null ? new Dictionary<string, string>() : _additionalLanguage.TextValues))
			{
				var additionalHeight = 0.0f;

				var additionalLanguageValue = string.Empty;
				if (_additionalLanguage != null &&
				    _additionalLanguage.TextValues.TryGetValue(kvp.Key, out additionalLanguageValue))
				{
					additionalHeight = GetStringHeight(additionalLanguageValue);
				}

				var maxHeight = Mathf.Max(GetStringHeight(kvp.Value), additionalHeight);

				GUILayout.BeginHorizontal();
				EditorGUILayout.SelectableLabel(kvp.Key, GUILayout.Width(width), GUILayout.Height(maxHeight));
				EditorGUILayout.SelectableLabel(kvp.Value, GUILayout.Width(width), GUILayout.Height(maxHeight));
				EditorGUILayout.SelectableLabel(additionalLanguageValue, GUILayout.Width(width),
					GUILayout.Height(maxHeight));
				GUILayout.EndHorizontal();

				totalHeight += maxHeight;
			}

			EditorGUILayout.EndScrollView();
		}

		private Dictionary<string, string> GetFilteredValues(Dictionary<string, string> rootValues,
			Dictionary<string, string> additionalValues)
		{
			if (rootValues == null || rootValues.Count <= 0) return new Dictionary<string, string>();
			if (!string.IsNullOrEmpty(_keyFilter))
			{
				rootValues = rootValues.Where(a => a.Key.ToLower().Contains(_keyFilter.Trim().ToLower()))
					.ToDictionary(a => a.Key, b => b.Value);
			}

			if (rootValues == null || rootValues.Count <= 0) return new Dictionary<string, string>();
			if (!string.IsNullOrEmpty(_rootLanguageFilter))
			{
				rootValues = rootValues.Where(a => a.Value.ToLower().Contains(_rootLanguageFilter.Trim().ToLower()))
					.ToDictionary(a => a.Key, b => b.Value);
			}

			if (rootValues == null || rootValues.Count <= 0 ||
			    additionalValues == null || additionalValues.Count <= 0) return new Dictionary<string, string>();
			if (!string.IsNullOrEmpty(_additionalLanguageFilter))
			{
				additionalValues = additionalValues
					.Where(a => a.Value.ToLower().Contains(_additionalLanguageFilter.Trim().ToLower()))
					.ToDictionary(a => a.Key, b => b.Value);
				rootValues = rootValues.Where(a => additionalValues.Any(b => b.Key == a.Key))
					.ToDictionary(a => a.Key, b => b.Value);
			}

			return rootValues;
		}

		private float GetStringHeight(string str)
		{
			var rowCount = 1;
			if (!string.IsNullOrEmpty(str))
			{
				rowCount = str.Count(a => a == '\n') + 1;
			}

			return 10 + rowCount * 13.0f;
		}

		private LocalizationLanguage LoadLanguage(string languageCode)
		{
			var configurationFile = Path.Combine(Application.dataPath,
				string.Format("Resources/{0}.json",
					string.Format(LocalizationConsts.TextLocalizationFolder, languageCode)));

			if (File.Exists(configurationFile))
			{
				return FileManager.LoadFromResources<LocalizationLanguage>(
					string.Format(LocalizationConsts.TextLocalizationFolder, string.Format("{0}.json", languageCode)));
			}

			return null;
		}
	}
}
