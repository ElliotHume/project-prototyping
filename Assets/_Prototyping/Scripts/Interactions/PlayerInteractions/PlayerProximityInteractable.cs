using System;
using _Prototyping.Interactions.Core;
using UnityEngine;

namespace _Prototyping.Interactions.PlayerInteractions
{
	public class PlayerProximityInteractable : MonoBehaviour, IInteractable<PlayerProximityInteractable>
	{
		#region IInteractable

		public IInteractor<PlayerProximityInteractable> currentInteractor { get; private set; }
		
		IInteractor<PlayerProximityInteractable> IInteractable<PlayerProximityInteractable>.CurrentInteractor
		{
			get => currentInteractor;
			set => currentInteractor = value;
		}

		public Action<IInteractor<PlayerProximityInteractable>> OnStartInteraction { get; }
		public Action<IInteractor<PlayerProximityInteractable>> OnFinishInteraction { get; }

		#endregion

		[field: SerializeField]
		[field: Tooltip("If left blank, will default to itself.")]
		public GameObject rootGameObject { get; private set; }

		private void Awake()
		{
			if (rootGameObject == null)
				rootGameObject = gameObject;
		}

		public void StartInteraction(IInteractor<PlayerProximityInteractable> interactor)
		{
			OnStartInteraction?.Invoke(interactor);
			currentInteractor = interactor;
		}

		public void FinishInteraction(IInteractor<PlayerProximityInteractable> interactor)
		{
			OnFinishInteraction?.Invoke(interactor);
			currentInteractor = null;
		}

		public bool CanBeInteractedWith(IInteractor<PlayerProximityInteractable> interactor)
		{
			return currentInteractor == null;
		}
	}
}