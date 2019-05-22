using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GSImporter
{
	public class Spreadsheet
	{
		[JsonProperty] public string version { get; private set; }
		[JsonProperty] public string reqId { get; private set; }
		[JsonProperty] public string status { get; private set; }
		[JsonProperty] public string sig { get; private set; }
		[JsonProperty] public Table table { get; private set; }
	}
}
