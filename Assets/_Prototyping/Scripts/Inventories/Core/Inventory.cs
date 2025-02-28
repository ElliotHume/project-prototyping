using System;
using System.Collections.Generic;
using _Prototyping.Utilities;
using UnityEngine;

namespace _Prototyping.Inventories.Core
{
	public class Inventory : MonoBehaviour, IHasUuid
	{
		public InventoryConfig config;
		
		public Guid uuid { get; set; }
		public Dictionary<Vector2Int, InventoryCompartment> compartments;

		private void Awake()
		{
			(this as IHasUuid).GenerateUuid();
		}

		private void Start()
		{
			InitializeCompartmentsFromConfig();
		}

		private void InitializeCompartmentsFromConfig()
		{
			
		}
	}
}