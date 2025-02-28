using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Inventories.Core
{
	[CreateAssetMenu(fileName = "InventoryCompartmentConfig", menuName = "PROTO/Inventory/InventoryCompartmentConfig")]
	public class InventoryCompartmentConfig : ScriptableObject
	{
		public string compartmentId;
		public Vector2Int compartmentDimensions;
		public List<ItemTag> tagWhitelist;
		public List<ItemTag> tagBlacklist;
	}
}