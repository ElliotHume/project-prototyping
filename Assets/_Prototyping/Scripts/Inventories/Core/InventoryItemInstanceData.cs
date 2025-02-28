using System;
using System.Collections.Generic;
using _Prototyping.Utilities;

namespace _Prototyping.Inventories.Core
{
	public class InventoryItemInstanceData : IHasUuid
	{
		public Guid uuid { get; set; }
		public Dictionary<string, string> instanceDataDict;
	}
}