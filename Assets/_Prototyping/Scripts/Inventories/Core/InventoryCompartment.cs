using System;
using System.Collections.Generic;
using _Prototyping.Utilities;
using UnityEngine;

namespace _Prototyping.Inventories.Core
{
	public class InventoryCompartment : IHasUuid
	{
		public Guid uuid { get; set; }
		public InventoryCompartmentConfig config;
		public Dictionary<Vector2Int, InventorySlot> slots;
		public Inventory inventory;

		public InventoryCompartment(Inventory inventory, InventoryCompartmentConfig config)
		{
			if (inventory == null || config == null)
				Debug.LogError($"[{nameof(InventoryCompartment)}] An InventoryItem was created with a null inventory or compartment config.");
			
			(this as IHasUuid).GenerateUuid();
			this.inventory = inventory;
			this.config = config;

			InitializeSlotsFromConfig();
		}

		public void InitializeSlotsFromConfig()
		{
			if (config == null)
			{
				Debug.LogError($"[{nameof(InventoryCompartment)}] Compartment config was null, cannot initialize slots.");
				return;
			}

			slots = new Dictionary<Vector2Int, InventorySlot>();
			Vector2Int coordinates;
			for (int i = 0; i < config.compartmentDimensions.x; i++)
			{
				for (int j = 0; j < config.compartmentDimensions.y; j++)
				{
					coordinates = new Vector2Int(i, j);
					slots.Add(coordinates, new InventorySlot(coordinates, this));
				}
			}
		}
	}
}