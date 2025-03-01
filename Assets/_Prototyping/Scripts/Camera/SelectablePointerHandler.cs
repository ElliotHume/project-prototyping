using _Prototyping.PointerSelectables.Core;
using UnityEngine;

namespace _Prototyping.Camera
{
	public class SelectablePointerHandler : MonoBehaviour
	{
		[SerializeField]
		private UnityEngine.Camera _mainCamera;

		[SerializeField]
		private LayerMask _selectableLayerMask;

		private IPointerSelectable _hoveredSelectable;
		private IPointerSelectable _selectedSelectable;
		
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

			if (hitSelectable != _hoveredSelectable)
			{
				if (_hoveredSelectable != null)
					_hoveredSelectable.EndHover();
				
				if (hitSelectable != null)
					hitSelectable.StartHover();

				_hoveredSelectable = hitSelectable;
			}
		}
		
		private void CheckSelection()
		{
			if (Input.GetMouseButtonDown(0))
			{
				if (_selectedSelectable != null)
					_selectedSelectable.EndSelection();
				
				if (_hoveredSelectable != null)
				{
					_selectedSelectable = _hoveredSelectable;
					_selectedSelectable.StartSelection();
				}
			}
		}
	}
}