using UnityEngine;

namespace _Prototyping.Inventories.Core
{
	public class InventorySlot
	{
		public Vector2Int coordinates;
		public InventoryCompartment compartment;
		public InventoryItem item = null;

		public bool hasItem => item == null;

		public InventorySlot(Vector2Int coordinates, InventoryCompartment compartment)
		{
			this.coordinates = coordinates;
			this.compartment = compartment;
		}
	}
}