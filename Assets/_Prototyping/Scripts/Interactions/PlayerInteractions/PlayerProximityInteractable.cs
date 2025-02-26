using System;
using _Prototyping.Interactions.Core;
using UnityEngine;

namespace _Prototyping.Interactions.PlayerInteractions
{
	public class PlayerProximityInteractable : MonoBehaviour, IInteractable<PlayerProximityInteractable>
	{
		#region IInteractable

		public PlayerProximityInteractor currentInteractor { get; private set; } = null;
		IInteractor<PlayerProximityInteractable> IInteractable<PlayerProximityInteractable>.CurrentInteractor => currentInteractor;

		public Action<IInteractor<PlayerProximityInteractable>> OnStartInteraction { get; }
		public Action<IInteractor<PlayerProximityInteractable>> OnEndInteraction { get; }
		public Action<IInteractor<PlayerProximityInteractable>> OnStartHover { get; }
		public Action<IInteractor<PlayerProximityInteractable>> OnEndHover { get; }

		#endregion

		[field: SerializeField]
		[field: Tooltip("If left blank, will default to itself")]
		public GameObject rootGameObject { get; private set; }
		
		[field: SerializeField]
		[field: Tooltip("If left blank, will default to using GetComponent<Collider>()")]
		public new Collider collider { get; private set; }

		private void Awake()
		{
			if (rootGameObject == null)
				rootGameObject = gameObject;
			
			if (collider == null)
				collider = GetComponentInParent<Collider>();
		}

		public virtual void StartInteraction(IInteractor<PlayerProximityInteractable> interactor)
		{
			OnStartInteraction?.Invoke(interactor);
			currentInteractor = interactor as PlayerProximityInteractor;
		}

		public virtual void EndInteraction(IInteractor<PlayerProximityInteractable> interactor)
		{
			OnEndInteraction?.Invoke(interactor);
			currentInteractor = null;
		}

		public virtual void StartHover(IInteractor<PlayerProximityInteractable> interactor)
		{
			OnStartHover?.Invoke(interactor);
		}

		public virtual void EndHover(IInteractor<PlayerProximityInteractable> interactor)
		{
			OnEndHover?.Invoke(interactor);
		}

		public virtual bool CanBeInteractedWith(IInteractor<PlayerProximityInteractable> interactor)
		{
			return currentInteractor == null;
		}
	}
}