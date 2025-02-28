using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Inventories.Core
{
	[CreateAssetMenu(fileName = "InventoriableItemConfig", menuName = "PROTO/Inventory/InventoriableItemConfig")]
	public class InventoriableItemConfig : ScriptableObject
	{
		public string itemId;
		public Vector2Int slotDimensions;
		public List<ItemTag> itemTags;
		public GameObject itemPrefab;
	}
}