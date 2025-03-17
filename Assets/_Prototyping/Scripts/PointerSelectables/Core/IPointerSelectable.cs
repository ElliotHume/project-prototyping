using UnityEngine.Events;

namespace _Prototyping.PointerSelectables.Core
{
	public interface IPointerSelectable
	{
		public bool canBeHovered { get; }
		public bool canBeSelected { get; }
		public bool isHovered { get; }
		public bool isSelected { get; }
		
		public UnityEvent OnHoverStartUnityEvent { get; set; }
		public UnityEvent OnHoverEndUnityEvent { get; set; }
		public UnityEvent OnSelectionStartUnityEvent { get; set; }
		public UnityEvent OnSelectionEndUnityEvent { get; set; }
		
		public void StartHover();
		public void EndHover();
		public void StartSelection();
		public void EndSelection();

		
	}
}