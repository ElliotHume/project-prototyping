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
				collider = GetComponent<Collider>();
		}

		public void StartInteraction(IInteractor<PlayerProximityInteractable> interactor)
		{
			OnStartInteraction?.Invoke(interactor);
			currentInteractor = interactor as PlayerProximityInteractor;
		}

		public void EndInteraction(IInteractor<PlayerProximityInteractable> interactor)
		{
			OnEndInteraction?.Invoke(interactor);
			currentInteractor = null;
		}

		public void StartHover(IInteractor<PlayerProximityInteractable> interactor)
		{
			OnStartHover?.Invoke(interactor);
		}

		public void EndHover(IInteractor<PlayerProximityInteractable> interactor)
		{
			OnEndHover?.Invoke(interactor);
		}

		public bool CanBeInteractedWith(IInteractor<PlayerProximityInteractable> interactor)
		{
			Debug.Log("CHECK IF CAN INTERACT", gameObject);
			return currentInteractor == null;
		}
	}
}