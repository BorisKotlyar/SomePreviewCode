using System.Collections.Generic;
using Newtonsoft.Json;

namespace Localization.Data
{
	public class LocalizationLanguage
	{
		[JsonProperty] public string LanguageCode { get; private set; }
		[JsonProperty] public Dictionary<string, string> TextValues { get; private set; }

		public LocalizationLanguage(string languageCode, Dictionary<string, string> textValues)
		{
			LanguageCode = languageCode;
			TextValues = textValues;
		}
	}
}
