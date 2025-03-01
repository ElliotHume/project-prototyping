using System;
using _Prototyping.Grids.Core;
using _Prototyping.PointerSelectables.Core;
using _Prototyping.Utilities;
using UnityEngine;
using UnityEngine.Events;

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
		
		#region ISelectable
		
		public bool canBeSelected { get; private set; } = true;
		public bool isHovered { get; private set; }
		public bool isSelected { get; private set; }
		[field: SerializeField]
		public UnityEvent OnHoverStart { get; set; }
		[field: SerializeField]
		public UnityEvent OnHoverEnd { get; set; }
		[field: SerializeField]
		public UnityEvent OnSelectionStart { get; set; }
		[field: SerializeField]
		public UnityEvent OnSelectionEnd { get; set; }

		public void StartHover()
		{
			isHovered = true;
			OnHoverStart?.Invoke();
			ApplyOutline();
		}

		public void EndHover()
		{
			isHovered = false;
			OnHoverEnd?.Invoke();
			ApplyOutline();
		}

		public void StartSelection()
		{
			isSelected = true;
			OnSelectionStart?.Invoke();
			ApplyOutline();
		}

		public void EndSelection()
		{
			isSelected = false;
			OnSelectionEnd?.Invoke();
			ApplyOutline();
		}
		
		#endregion

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