using UnityEngine;

namespace _Prototyping.Inventories.Core
{
	public class InventorySlot
	{
		public Vector2Int coordinates;
		public InventoryCompartment compartment;
		public InventoryItem item;

		public bool hasItem => item == null;
	}
}