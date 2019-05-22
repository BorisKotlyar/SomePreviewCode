using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GSImporter
{
	public class Row
	{
		[JsonProperty] public List<Cell> c { get; private set; }
	}
}
