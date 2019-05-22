using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace GSImporter
{
	public class Table
	{
		[JsonProperty] public List<Col> cols { get; private set; }
		[JsonProperty] public List<Row> rows { get; private set; }
	}
}