using UnityEngine;

namespace _Prototyping.PointerSelectables.Core
{
	public interface IPointerSelectable
	{
		public bool canBeSelected { get; }
		public bool isHovered { get; }
		public bool isSelected { get; }
		public void StartHover();
		public void EndHover();
		public void StartSelection();
		public void EndSelection();
	}
}