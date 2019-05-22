using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GSImporter
{
	public class Importer
	{
		private const string SpreadsheetDownloadFormatUrl =
			"https://docs.google.com/spreadsheets/d/{0}/gviz/tq?tqx=out:json&sheet={1}";
		private const string ResponceStartString = "setResponse(";
		private const string ResponceEndString = ");";

		public static void Import(string spreadsheetId, string sheetName, System.Action<string, Spreadsheet> callback)
		{
			if(string.IsNullOrEmpty(spreadsheetId) || string.IsNullOrEmpty(sheetName))
			{
				Debug.Log (string.Format("GSImporter error:{0}{1}",
						string.IsNullOrEmpty(spreadsheetId) ? "\nspreadsheetId is empty" : "",
						string.IsNullOrEmpty(sheetName) ? "\nsheetName is empty" : "").Color (Color.red));
				return;
			}
			if(callback==null)
			{
				Debug.Log ("GSImporter error:\nImport callback is null".Color (Color.red));
				return;
			}

			EditorCoroutine.Start(LoadSpreadsheet(spreadsheetId, sheetName, callback));
		}

		private static IEnumerator LoadSpreadsheet(string spreadsheetId, string spreadsheetName,
			System.Action<string, Spreadsheet> callback)
		{
			var spreadsheetUrl = string.Format(SpreadsheetDownloadFormatUrl, spreadsheetId, spreadsheetName);
			var www = new WWW(spreadsheetUrl);

			while(!www.isDone)
			{
				yield return null;	
			}

			if(string.IsNullOrEmpty(www.error))
			{
				var json = www.text;
				
				var responceStartId = json.IndexOf(ResponceStartString);
				var responceEndId = json.LastIndexOf(ResponceEndString);

				if (responceStartId < 0 || responceEndId < 0)
				{
					("GSImporter error: Responce format error\nВозможно проблемы в неправильной ссылке на документ" +
						" или же в том что он не открыт для всех в интернете").LogEditor(Color.red);
					yield break;
				}

				json = json.Remove(responceEndId, ResponceEndString.Length);
				json = json.Remove(0, responceStartId + ResponceStartString.Length);

				try
				{
					callback(spreadsheetName, JsonConvert.DeserializeObject<Spreadsheet>(json));
				}
				catch (Exception)
				{
					"GSImporter error: Responce deserialize error".LogEditor(Color.red);
				}
			}
			else
			{
				Debug.Log (string.Format("GSImporter error:\nSpreadsheet with URL: {0}\nfailed to load with error: {1}",
						spreadsheetUrl, www.error).Color (Color.red));
			}
		}
	}
}
