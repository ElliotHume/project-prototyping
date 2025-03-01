using UnityEngine.Events;

namespace _Prototyping.PointerSelectables.Core
{
	public interface IPointerSelectable
	{
		public bool canBeHovered { get; }
		public bool canBeSelected { get; }
		public bool isHovered { get; }
		public bool isSelected { get; }
		
		public UnityEvent OnHoverStart { get; set; }
		public UnityEvent OnHoverEnd { get; set; }
		public UnityEvent OnSelectionStart { get; set; }
		public UnityEvent OnSelectionEnd { get; set; }
		
		public void StartHover();
		public void EndHover();
		public void StartSelection();
		public void EndSelection();

		
	}
}