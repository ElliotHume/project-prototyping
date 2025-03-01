using _Prototyping.Grids.Core;
using _Prototyping.PointerSelectables.Core;
using _Prototyping.Utilities;
using UnityEngine;

namespace _Prototyping.Grids
{
	public class GridCellObject : MonoBehaviour, IHasGridPosition<GridBoardCell>, IPointerSelectable
	{
		[SerializeField] private Outline _outline;
		[SerializeField] private OutlineSettingsConfig _hoveredOutlineSettingsConfig;
		[SerializeField] private OutlineSettingsConfig _selectedOutlineSettingsConfig;
		public GridBoardCell cell { get; private set; }
		public IGridCell<GridBoardCell> Cell => cell;

		public IGrid<GridBoardCell> grid => cell.grid;
		public Vector2Int gridCoordinates => cell.gridCoordinates;
		public int x => gridCoordinates.x;
		public int y => gridCoordinates.y;
		
		public bool canBeSelected { get; private set; } = true;
		public bool isHovered { get; private set; }
		public bool isSelected { get; private set; }
		
		public void StartHover()
		{
			isHovered = true;
			ApplyOutline();
		}

		public void EndHover()
		{
			isHovered = false;
			ApplyOutline();
		}

		public void StartSelection()
		{
			isSelected = true;
			ApplyOutline();
		}

		public void EndSelection()
		{
			isSelected = false;
			ApplyOutline();
		}

		public void ApplyOutline()
		{
			bool showOutline = isSelected || isHovered;
			_outline.enabled = showOutline;

			if (showOutline)
			{
				OutlineSettingsConfig outlineConfig =
					isSelected ? _selectedOutlineSettingsConfig : _hoveredOutlineSettingsConfig;
				_outline.OutlineColor = outlineConfig.outlineColor;
				_outline.OutlineMode = outlineConfig.outlineMode;
				_outline.OutlineWidth = outlineConfig.outlineWidth;
			}
		}
	}
}