using System;
using System.Collections.Generic;
using _Prototyping.Grids.Core;
using UnityEngine;

namespace _Prototyping.Grids
{
	public class GridBoardCell : MonoBehaviour, IGridCell<GridBoardCell>
	{
		public bool doesChangeMaterialWithPosition = true;

		[SerializeField] private List<Renderer> _renderersToChangeMaterial;
		[SerializeField] private Material _baseMaterial;
		[SerializeField] private Material _alternateMaterial;
		[SerializeField] private Outline _outline;

		public IGrid<GridBoardCell> grid { get; private set; }
		public Vector2Int gridCoordinates { get; private set; }
		public int x => gridCoordinates.x;
		public int y => gridCoordinates.y;

		private bool _isHighlighted = false;

		public void Instantiate(GridBoard grid, Vector2Int coordinates)
		{
			this.grid = grid;
			this.gridCoordinates = coordinates;

			ToggleHighlight(_isHighlighted);

			if (doesChangeMaterialWithPosition)
				ChangeColourWithPosition();
		}

		private void ChangeColourWithPosition()
		{
			foreach (Renderer renderer in _renderersToChangeMaterial)
			{
				if ((x % 2 == 0 && y % 2 != 0) || (x % 2 != 0 && y % 2 == 0))
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