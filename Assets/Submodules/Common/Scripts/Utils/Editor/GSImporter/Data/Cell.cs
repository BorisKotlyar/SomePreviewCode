using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GSImporter
{
	public class Cell
	{
		[JsonProperty] public string v { get; private set; }
	}
}
