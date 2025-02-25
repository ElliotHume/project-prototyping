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
		private OutlineSettingsConfig _outlineSettings;

		[CanBeNull]
		private PlayerProximityInteractable _outlinedTarget = null;

		private Outline _outline;

		private void Update()
		{
			PlayerProximityInteractable currentInteractable = _proximityInteractor.currentInteractable;
			if (currentInteractable != _outlinedTarget)
			{
				if (_outlinedTarget != null)
					RemoveOutlineFromTarget();

				if (currentInteractable != null)
					AddOutlineToTarget(currentInteractable);

				_outlinedTarget = _proximityInteractor.currentInteractable;
			}
		}

		private void RemoveOutlineFromTarget()
		{
			if (_outline != null)
			{
				Destroy(_outline);
				_outline = null;
			}
		}

		private void AddOutlineToTarget(PlayerProximityInteractable interactable)
		{
			_outline = interactable.gameObject.AddComponent<Outline>();
			_outline.OutlineMode = _outlineSettings.outlineMode;
			_outline.OutlineColor = _outlineSettings.outlineColor;
			_outline.OutlineWidth = _outlineSettings.outlineWidth;
		}
	}
}