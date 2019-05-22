using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GSImporter
{
	public class Col
	{
		[JsonProperty] public string id { get; private set; }
		[JsonProperty] public string label { get; private set; }
		[JsonProperty] public string type { get; private set; }
	}
}
