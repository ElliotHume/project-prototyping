using System;
using JetBrains.Annotations;

namespace _Prototyping.Interactions.Core
{
	public interface IInteractable<T> where T : IInteractable<T>
	{
		[CanBeNull]
		public IInteractor<T> CurrentInteractor { get;}
		
		public Action<IInteractor<T>> OnStartInteraction { get; }
		public Action<IInteractor<T>> OnEndInteraction { get; }
		
		public Action<IInteractor<T>> OnStartHover { get; }
		public Action<IInteractor<T>> OnEndHover { get; }
		
		public void StartInteraction(IInteractor<T> interactor);
		
		public void EndInteraction(IInteractor<T> interactor);
		
		public void StartHover(IInteractor<T> interactor);
		
		public void EndHover(IInteractor<T> interactor);

		public bool CanBeInteractedWith(IInteractor<T> interactor);
		
	}
}