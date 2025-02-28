using System;
using _Prototyping.Utilities;
using UnityEngine;

namespace _Prototyping.Inventories.Core
{
	public class InventoryItem : IHasUuid
	{
		public Guid uuid { get; set; }
		public InventoriableItemConfig item;
		public InventorySlot slot;
		
		public InventoryItemInstanceData instanceData;
		
		public InventoryItem(InventoriableItemConfig item, InventorySlot slot, InventoryItemInstanceData instanceData = null)
		{
			if (item == null || slot == null)
				Debug.LogError($"[{nameof(InventoryItem)}] An InventoryItem was created with a null item or slot.");

			(this as IHasUuid).GenerateUuid();
			this.item = item;
			this.slot = slot;
			this.instanceData = instanceData;
		}
	}
}