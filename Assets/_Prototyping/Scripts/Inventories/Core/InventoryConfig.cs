using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Prototyping.Inventories.Core
{
	[CreateAssetMenu(fileName = "InventoryConfig", menuName = "PROTO/Inventory/InventoryConfig")]
	public class InventoryConfig : ScriptableObject
	{
		public string inventoryId;
		public SerializedDictionary<Vector2Int, InventoryCompartmentConfig> compartmentsConfigs;
	}
}