using System;
using JetBrains.Annotations;

namespace _Prototyping.Interactions.Core
{
	public interface IInteractor<T> where T : IInteractable<T>
	{
		[CanBeNull]
		public T CurrentInteractable { get; internal set; }
		
		public Action<T> OnStartInteraction { get; }
		public Action<T> OnFinishInteraction { get; }
		
		public void StartInteraction(T interactable);

		public void FinishInteraction(T interactable);

		public bool CanInteractWith(T interactable);
	}
}