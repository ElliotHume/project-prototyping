using System;
using JetBrains.Annotations;

namespace _Prototyping.Interactions.Core
{
	public interface IInteractor<T> where T : IInteractable<T>
	{
		[CanBeNull]
		public T CurrentInteractable { get; }
		
		[CanBeNull]
		public T HoveredInteractable { get; }
		
		public Action<T> OnStartInteraction { get; set; }
		public Action<T> OnEndInteraction { get; set; }
		
		public Action<T> OnStartHover { get; set; }
		public Action<T> OnEndHover { get; set; }
		
		public void StartInteraction(T interactable);

		public void EndInteraction(T interactable);
		
		public void StartHover(T interactable);

		public void EndHover(T interactable);

		public bool CanInteractWith(T interactable);
	}
}