using System;
using System.Collections.Generic;
using System.IO;
using Common.Utils;
using GSImporter;
using Localization.Data;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

namespace Localization.Editor
{
	public static class LocalizationImporter
	{
		public static event Action OnLocalizationImported = () => { };

		public static void Import()
		{
			if (string.IsNullOrEmpty(LocalizationConnectionSettings.SpreadsheetApiKey) ||
			    string.IsNullOrEmpty(LocalizationConnectionSettings.SpreadsheetSheetName)) return;

			Importer.Import(LocalizationConnectionSettings.SpreadsheetApiKey,
				LocalizationConnectionSettings.SpreadsheetSheetName, LocalizationImported);
		}

		private static void LocalizationImported(string sheetName, Spreadsheet spreadsheet)
		{
			var titleRow = spreadsheet.table.rows[0];
			var supportedLanguages = new List<string>();

			for (var colId = 1; colId < titleRow.c.Count; colId++)
			{
				if (titleRow.c == null || titleRow.c[colId] == null || titleRow.c[colId].v == null)
				{
					continue;
				}

				var languageCode = titleRow.c[colId].v.ToLower();
				supportedLanguages.Add(languageCode);
				var textValues = new Dictionary<string, string>();

				for (var rowId = 1; rowId < spreadsheet.table.rows.Count; rowId++)
				{
					var currentRow = spreadsheet.table.rows[rowId];
					if (currentRow.c[0].IsNullOrEmpty()) continue;
					
					var key = currentRow.c[0].v;
					var value = currentRow.c[colId].IsNullOrEmpty() ? string.Empty : currentRow.c[colId].v;

					textValues.Add(key, value);
				}
				
				FileManager.SaveToResources(
					string.Format(LocalizationConsts.TextLocalizationFolder, string.Format("{0}.json", languageCode)),
					new LocalizationLanguage(languageCode, textValues));
			}
			
			new LocalizationLanguagesSettings().SupportedLanguages = supportedLanguages;

			RefreshAndroidAppName();
			
			AssetDatabase.Refresh();
			OnLocalizationImported.Invoke();
			
			"Localization import successes".LogEditor(Color.cyan);
		}

		private static void RefreshAndroidAppName()
		{
			var languagesSettings = new LocalizationLanguagesSettings();
			foreach (var languageCode in languagesSettings.SupportedLanguages)
			{
				var language = FileManager.LoadFromResources<LocalizationLanguage>(
					string.Format(LocalizationConsts.TextLocalizationFolder, string.Format("{0}.json", languageCode)));

				string value;
				if (!language.TextValues.TryGetValue("AppIconName", out value)) continue;

				switch (languageCode)
				{
					case "fa-ir":
						CreateAndroidIconLocalization("fa", value);
						break;
					case "zh-tw":
						CreateAndroidIconLocalization("zh-rTW", value);
						if (!languagesSettings.SupportedLanguages.Contains("zh-hk"))
						{
							CreateAndroidIconLocalization("zh-rHK", value);
						}
						break;
					case "zh-hk":
						CreateAndroidIconLocalization("zh-rHK", value);
						break; 
					case "ru":
						CreateAndroidIconLocalization(languageCode, value);
						if (!languagesSettings.SupportedLanguages.Contains("be"))
						{
							CreateAndroidIconLocalization("be", value);
						}
						if (!languagesSettings.SupportedLanguages.Contains("uk"))
						{
							CreateAndroidIconLocalization("uk", value);
						}
						break;
					default:
						CreateAndroidIconLocalization(languageCode, value);
						break;
				}
			}
		}
		
		private static void CreateAndroidIconLocalization(string languageCode, string value)
		{
			var folderName = string.Format("values{0}",
				languageCode == "en" ? string.Empty : string.Format("-{0}", languageCode));
			var folderFullPath = string.Format("{0}/Plugins/Android/res/{1}", Application.dataPath, folderName);

			if (!Directory.Exists(folderFullPath))
			{
				Directory.CreateDirectory(folderFullPath);
			}

			var templateFileName =
				string.Format("{0}/Submodules/Common/Templates/Android/res/values/strings.xml", Application.dataPath);
			var targetFileName = Path.Combine(folderFullPath, "strings.xml");
			File.Copy(templateFileName, targetFileName, true);
			
			var streamReader = new StreamReader(targetFileName);
			var data = streamReader.ReadToEnd();
			streamReader.Close();
			
			var streamWriter = new StreamWriter(targetFileName);
			streamWriter.Write(data.Replace("{app_icon_description}", value));
			streamWriter.Close();
		}
	}
}