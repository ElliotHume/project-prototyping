using System;
using _Prototyping.Interactions.Core;
using UnityEngine;

namespace _Prototyping.Interactions.PlayerInteractions
{
	public class PlayerPickupInteractable : MonoBehaviour, IInteractable<PlayerPickupInteractable>
	{
		#region IInteractable

		public PlayerPickupInteractor currentInteractor { get; private set; } = null;
		IInteractor<PlayerPickupInteractable> IInteractable<PlayerPickupInteractable>.CurrentInteractor => currentInteractor;

		public Action<IInteractor<PlayerPickupInteractable>> OnStartInteraction { get; }
		public Action<IInteractor<PlayerPickupInteractable>> OnEndInteraction { get; }
		public Action<IInteractor<PlayerPickupInteractable>> OnStartHover { get; }
		public Action<IInteractor<PlayerPickupInteractable>> OnEndHover { get; }

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

		public virtual void StartInteraction(IInteractor<PlayerPickupInteractable> interactor)
		{
			OnStartInteraction?.Invoke(interactor);
			currentInteractor = interactor as PlayerPickupInteractor;

			Rigidbody rb = collider.attachedRigidbody;
			
			rb.isKinematic = true;
			rb.useGravity = false;
			rb.linearVelocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

		public virtual void EndInteraction(IInteractor<PlayerPickupInteractable> interactor)
		{
			OnEndInteraction?.Invoke(interactor);
			currentInteractor = null;
			
			Rigidbody rb = collider.attachedRigidbody;
			
			rb.isKinematic = false;
			rb.useGravity = true;
			rb.linearVelocity = Vector3.zero;
			rb.angularVelocity = Vector3.zero;
		}

		public virtual void StartHover(IInteractor<PlayerPickupInteractable> interactor)
		{
			OnStartHover?.Invoke(interactor);
		}

		public virtual void EndHover(IInteractor<PlayerPickupInteractable> interactor)
		{
			OnEndHover?.Invoke(interactor);
		}

		public virtual bool CanBeInteractedWith(IInteractor<PlayerPickupInteractable> interactor)
		{
			return currentInteractor == null;
		}
	}
}