using System.Collections.Generic;
using _Prototyping.Utilities;
using JetBrains.Annotations;
using UnityEngine;

namespace _Prototyping.Interactions.PlayerInteractions
{
	public class PlayerProximityInteractionOutliner : MonoBehaviour
	{
		[SerializeField]
		private PlayerProximityInteractor _proximityInteractor;

		[SerializeField]
		private OutlineSettingsConfig _hoveredOutlineSettings;
		[SerializeField]
		private OutlineSettingsConfig _selectedOutlineSettings;

		private Dictionary<PlayerProximityInteractable, Outline> _outlines;

		private void Start()
		{
			_proximityInteractor.OnStartHover += OnStartHover;
			_proximityInteractor.OnEndHover += OnEndHover;
			
			_proximityInteractor.OnStartInteraction += OnStartInteraction;
			_proximityInteractor.OnEndInteraction += OnEndInteraction;

			_outlines = new Dictionary<PlayerProximityInteractable, Outline>();
		}

		private void OnStartHover(PlayerProximityInteractable interactable)
		{
			AddOutlineToTarget(interactable, _hoveredOutlineSettings);
		}
		
		private void OnEndHover(PlayerProximityInteractable interactable)
		{
			RemoveOutlineFromTarget(interactable);
		}
		
		private void OnStartInteraction(PlayerProximityInteractable interactable)
		{
			AddOutlineToTarget(interactable, _selectedOutlineSettings);
		}
		
		private void OnEndInteraction(PlayerProximityInteractable interactable)
		{
			// Reset back to a hovered outline, only if the outline is still present
			if (_outlines.TryGetValue(interactable, out Outline outline))
			{
				outline.OutlineMode = _hoveredOutlineSettings.outlineMode;
				outline.OutlineColor = _hoveredOutlineSettings.outlineColor;
				outline.OutlineWidth = _hoveredOutlineSettings.outlineWidth;
			}
		}

		private void RemoveOutlineFromTarget(PlayerProximityInteractable interactable)
		{
			if (_outlines.TryGetValue(interactable, out Outline outline))
			{
				Destroy(outline);
				_outlines.Remove(interactable);
			}
		}

		private void AddOutlineToTarget(PlayerProximityInteractable interactable, OutlineSettingsConfig outlineSettingsConfig)
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
				Outline newOutline = interactable.gameObject.AddComponent<Outline>();
				newOutline.OutlineMode = outlineSettingsConfig.outlineMode;
				newOutline.OutlineColor = outlineSettingsConfig.outlineColor;
				newOutline.OutlineWidth = outlineSettingsConfig.outlineWidth;
				
				_outlines.Add(interactable, newOutline);
			}
			
		}
	}
}