using _Prototyping.Chess;
using _Prototyping.PointerSelectables.Core;
using UnityEngine;
using UnityEngine.Events;

namespace _Prototyping.PointerSelectables
{
	public class ChessBoardCellSelectable : MonoBehaviour, IPointerSelectable
	{
		[field: SerializeField]
		public ChessBoardCell cell { get; private set; }
		
		#region ISelectable
		
		public bool canBeHovered { get; private set; } = true;
		public bool canBeSelected { get; private set; } = true;
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

		private void Start()
		{
			if (cell == null)
				cell = GetComponentInParent<ChessBoardCell>();
		}

		public void StartHover()
		{
			isHovered = true;
			OnHoverStartUnityEvent?.Invoke();
		}

		public void EndHover()
		{
			isHovered = false;
			OnHoverEndUnityEvent?.Invoke();
		}

		public void StartSelection()
		{
			isSelected = true;
			OnSelectionStartUnityEvent?.Invoke();
		}

		public void EndSelection()
		{
			isSelected = false;
			OnSelectionEndUnityEvent?.Invoke();
		}
		
		#endregion
	}
}