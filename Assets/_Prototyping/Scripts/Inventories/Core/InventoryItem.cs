using System;
using _Prototyping.Utilities;

namespace _Prototyping.Inventories.Core
{
	public class InventoryItem : IHasUuid
	{
		public InventoriableItemConfig item;
		public InventorySlot slot;
		public Guid uuid { get; set; }
	}
}