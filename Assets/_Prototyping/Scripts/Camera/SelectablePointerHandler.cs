using System;
using _Prototyping.PointerSelectables.Core;
using UnityEngine;

namespace _Prototyping.Camera
{
	public class SelectablePointerHandler : MonoBehaviour
	{
		public static SelectablePointerHandler Instance;
		
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

		private void Awake()
		{
			SelectablePointerHandler.Instance = this;
		}
		
		private void Update()
		{
			Ray ray = _mainCamera.ScreenPointToRay(Input.mousePosition);
			CheckHovers(ray);
			CheckSelection();
		}

		private void CheckHovers(Ray mousePositionRay)
		{
			IPointerSelectable hitSelectable = null;
			if (Physics.Raycast(mousePositionRay, out RaycastHit hit, 100, _selectableLayerMask, QueryTriggerInteraction.Collide))
			{
				IPointerSelectable selectable = hit.collider.gameObject.GetComponentInParent<IPointerSelectable>();
				if (selectable != null && selectable.canBeSelected)
					hitSelectable = selectable;
			}

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
		
		private void CheckSelection()
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
				
				if (hoveredSelectable != null)
				{
					selectedSelectable = hoveredSelectable;
					selectedSelectable.StartSelection();
					OnPointerSelectionStarted?.Invoke(selectedSelectable);
				}
			}
		}
	}
}