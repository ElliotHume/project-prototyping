using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess
{
	public class ChessTileVisuals : MonoBehaviour
	{
		[SerializeField] private List<Renderer> _renderersToChangeMaterial;
		[SerializeField] private Material _baseMaterial;
		[SerializeField] private Material _alternateMaterial;
		[SerializeField] private Outline _outline;
		
		public bool doesChangeMaterialWithPosition = true;
		
		private bool _isHighlighted = false;
		private ChessBoardCell _cell;

		public void UpdateVisuals(ChessBoardCell cell)
		{
			_cell = cell;
			ToggleHighlight(_isHighlighted);

			if (doesChangeMaterialWithPosition)
				ChangeColourWithPosition();
		}
		
		private void ChangeColourWithPosition()
		{
			foreach (Renderer renderer in _renderersToChangeMaterial)
			{
				if ((_cell.x % 2 == 0 && _cell.y % 2 != 0) || (_cell.x % 2 != 0 && _cell.y % 2 == 0))
				{
					renderer.material = _alternateMaterial;
				}
				else
				{
					renderer.material = _baseMaterial;
				}
			}
		}

		public void ToggleHighlight(bool toggle)
		{
			_outline.enabled = toggle;
			_isHighlighted = toggle;
		}
	}
}