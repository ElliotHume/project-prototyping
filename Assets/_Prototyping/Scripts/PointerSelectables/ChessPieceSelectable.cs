using _Prototyping.Chess;
using _Prototyping.PointerSelectables.Core;
using _Prototyping.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace _Prototyping.PointerSelectables
{
	public class ChessPieceSelectable : MonoBehaviour, IPointerSelectable
	{
		[field: SerializeField]
		public ChessPiece chessPiece { get; private set; }
		
		[SerializeField] private Outline _outline;
		[SerializeField] private OutlineSettingsConfig _hoveredOutlineSettingsConfig;
		[SerializeField] private OutlineSettingsConfig _selectedOutlineSettingsConfig;
		
		#region ISelectable
		
		public bool canBeHovered { get; private set; } = true;
		public bool canBeSelected => (!chessPiece.chessManager || chessPiece.chessManager.canPlayerAct);
		public bool isHovered { get; private set; }
		public bool isSelected { get; private set; }
		[field: SerializeField]
		public UnityEvent OnHoverStartUnityEvent { get; set; }
		[field: SerializeField]
		public UnityEvent OnHoverEndUnityEvent { get; set; }
		[field: SerializeField]
		public UnityEvent OnSelectionStartUnityEvent { get; set; }
		[field: SerializeField]
		public UnityEvent OnSelectionEndUnityEvent { get; set; }

		public void StartHover()
		{
			isHovered = true;
			OnHoverStartUnityEvent?.Invoke();
			ApplyOutline();
		}

		public void EndHover()
		{
			isHovered = false;
			OnHoverEndUnityEvent?.Invoke();
			ApplyOutline();
		}

		public void StartSelection()
		{
			isSelected = true;
			OnSelectionStartUnityEvent?.Invoke();
			ApplyOutline();
		}

		public void EndSelection()
		{
			isSelected = false;
			OnSelectionEndUnityEvent?.Invoke();
			ApplyOutline();
		}

		#endregion

		private void Start()
		{
			if (chessPiece == null)
				chessPiece = GetComponentInParent<ChessPiece>();
		}
		
		public void ApplyOutline()
		{
			if (_outline == null)
				return;
			
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