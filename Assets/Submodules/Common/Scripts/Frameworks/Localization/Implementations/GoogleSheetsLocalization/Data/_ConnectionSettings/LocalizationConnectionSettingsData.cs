using Newtonsoft.Json;

namespace Localization.Data
{
	public class LocalizationConnectionSettingsData
	{
		[JsonProperty] public string SpreadsheetApiKey = string.Empty;
		[JsonProperty] public string SpreadsheetSheetName = string.Empty;
	}
}
