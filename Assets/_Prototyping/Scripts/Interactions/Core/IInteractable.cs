using System;
using JetBrains.Annotations;

namespace _Prototyping.Interactions.Core
{
	public interface IInteractable<T> where T : IInteractable<T>
	{
		[CanBeNull]
		public IInteractor<T> CurrentInteractor { get; internal set; }
		
		public Action<IInteractor<T>> OnStartInteraction { get; }
		public Action<IInteractor<T>> OnFinishInteraction { get; }
		
		public void StartInteraction(IInteractor<T> interactor);
		
		public void FinishInteraction(IInteractor<T> interactor);

		public bool CanBeInteractedWith(IInteractor<T> interactor);
	}
}