using System.Collections.Generic;
using _Prototyping.Utilities;
using JetBrains.Annotations;
using UnityEngine;

namespace _Prototyping.Interactions.PlayerInteractions
{
	public class PlayerPickupInteractionOutliner : MonoBehaviour
	{
		[SerializeField]
		private PlayerPickupInteractor _pickupInteractor;

		[SerializeField]
		private OutlineSettingsConfig _hoveredOutlineSettings;

		private Dictionary<PlayerPickupInteractable, Outline> _outlines;

		private void Start()
		{
			_pickupInteractor.OnStartHover += OnStartHover;
			_pickupInteractor.OnEndHover += OnEndHover;

			_outlines = new Dictionary<PlayerPickupInteractable, Outline>();
		}

		private void OnStartHover(PlayerPickupInteractable interactable)
		{
			AddOutlineToTarget(interactable, _hoveredOutlineSettings);
		}
		
		private void OnEndHover(PlayerPickupInteractable interactable)
		{
			RemoveOutlineFromTarget(interactable);
		}

		private void RemoveOutlineFromTarget(PlayerPickupInteractable interactable)
		{
			if (_outlines.TryGetValue(interactable, out Outline outline))
			{
				Destroy(outline);
				_outlines.Remove(interactable);
			}
		}

		private void AddOutlineToTarget(PlayerPickupInteractable interactable, OutlineSettingsConfig outlineSettingsConfig)
		{
			if (interactable == null)
				return;
			
			if (_outlines.TryGetValue(interactable, out Outline outline))
			{
				outline.OutlineMode = outlineSettingsConfig.outlineMode;
				outline.OutlineColor = outlineSettingsConfig.outlineColor;
				outline.OutlineWidth = outlineSettingsConfig.outlineWidth;
			}
			else
			{
				Outline newOutline = interactable.rootGameObject.gameObject.AddComponent<Outline>();
				newOutline.OutlineMode = outlineSettingsConfig.outlineMode;
				newOutline.OutlineColor = outlineSettingsConfig.outlineColor;
				newOutline.OutlineWidth = outlineSettingsConfig.outlineWidth;
				
				_outlines.Add(interactable, newOutline);
			}
			
		}
	}
}