using _Prototyping.PointerSelectables.Core;
using UnityEngine;
using UnityEngine.Events;

namespace _Prototyping.PointerSelectables
{
	public class ChessBoardCellSelectable : MonoBehaviour, IPointerSelectable
	{
		#region ISelectable
		
		public bool canBeHovered { get; private set; } = true;
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
		}

		public void EndHover()
		{
			isHovered = false;
			OnHoverEnd?.Invoke();
		}

		public void StartSelection()
		{
			isSelected = true;
			OnSelectionStart?.Invoke();
		}

		public void EndSelection()
		{
			isSelected = false;
			OnSelectionEnd?.Invoke();
		}
		
		#endregion
	}
}