using System.Collections.Generic;
using Newtonsoft.Json;

namespace Localization.Data
{
	public class LocalizationLanguagesSettingsData
	{
		[JsonProperty] public string DefaultLanguageCode { get; set; }
		[JsonProperty] public bool AutoDetectLanguage { get; set; }
		[JsonProperty] public List<string> SupportedLanguages { get; set; }
	}
}