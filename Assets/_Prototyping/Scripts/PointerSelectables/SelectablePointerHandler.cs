using System;
using _Prototyping.PointerSelectables.Core;
using UnityEngine;

namespace _Prototyping.PointerSelectables
{
	public class SelectablePointerHandler : MonoBehaviour
	{
		[SerializeField]
		private UnityEngine.Camera _mainCamera;

		[SerializeField]
		private LayerMask _selectableLayerMask;

		public IPointerSelectable hoveredSelectable { get; private set; }
		public Action<IPointerSelectable> OnPointerHoverStarted;
		public Action<IPointerSelectable> OnPointerHoverEnded;
		
		public IPointerSelectable selectedSelectable { get; private set; }
		public Action<IPointerSelectable> OnPointerSelectionStarted;
		public Action<IPointerSelectable> OnPointerSelectionEnded;
		public IPointerSelectable previousSelectedSelectable { get; private set; }

		private void Update()
		{
			IPointerSelectable hitSelectable = GetRaycastHitSelectable();
			CheckHovers(hitSelectable);
			CheckSelection(hitSelectable);
		}

		private IPointerSelectable GetRaycastHitSelectable()
		{
			Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			IPointerSelectable hitSelectable = null;
			if (Physics.Raycast(ray, out RaycastHit hit, 100, _selectableLayerMask, QueryTriggerInteraction.Collide))
			{
				IPointerSelectable selectable = hit.collider.gameObject.GetComponentInParent<IPointerSelectable>();
				if (selectable != null && selectable.canBeHovered)
					hitSelectable = selectable;
			}
			return hitSelectable;
		}

		private void CheckHovers(IPointerSelectable hitSelectable)
		{
			if (hitSelectable != hoveredSelectable)
			{
				if (hoveredSelectable != null)
				{
					hoveredSelectable.EndHover();
					OnPointerHoverEnded?.Invoke(hitSelectable);
				}
				
				if (hitSelectable != null)
				{
					hitSelectable.StartHover();
					OnPointerHoverStarted?.Invoke(hitSelectable);
				}

				hoveredSelectable = hitSelectable;
			}
		}
		
		private void CheckSelection(IPointerSelectable hitSelectable)
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (selectedSelectable != null)
				{
					selectedSelectable.EndSelection();
					OnPointerSelectionEnded?.Invoke(selectedSelectable);
				}

				previousSelectedSelectable = selectedSelectable;
				selectedSelectable = null;
				
				if (hitSelectable != null && hitSelectable.canBeSelected)
				{
					selectedSelectable = hitSelectable;
					selectedSelectable.StartSelection();
					OnPointerSelectionStarted?.Invoke(selectedSelectable);
				}
			}
		}
	}
}