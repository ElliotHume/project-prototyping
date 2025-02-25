using System;
using _Prototyping.Interactions.Core;
using UnityEngine;

namespace _Prototyping.Interactions.PlayerInteractions
{
	public class PlayerProximityInteractor : MonoBehaviour, IInteractor<PlayerProximityInteractable>
	{
		#region IInteractor

		public PlayerProximityInteractable currentInteractable { get; private set; }
		PlayerProximityInteractable IInteractor<PlayerProximityInteractable>.CurrentInteractable
		{
			get => currentInteractable;
			set => currentInteractable = value;
		}
		public Action<PlayerProximityInteractable> OnStartInteraction { get; }
		public Action<PlayerProximityInteractable> OnFinishInteraction { get; }
		
		#endregion

		[SerializeField]
		private float _proximityDistance = 1f;

		[SerializeField]
		private LayerMask _proximityCheckLayerMask;

		public bool isInteracting => currentInteractable != null;
		
		private float _sqrProxDistance;
		private Collider[] _sphereCastResults;

		private void Awake()
		{
			_sqrProxDistance = _proximityDistance * _proximityDistance;
			_sphereCastResults = new Collider[]{};
		}

		public void StartInteraction(PlayerProximityInteractable interactable)
		{
			OnStartInteraction?.Invoke(interactable);
			currentInteractable = interactable;
		}

		public void FinishInteraction(PlayerProximityInteractable interactable)
		{
			OnFinishInteraction?.Invoke(interactable);
			currentInteractable = null;
		}

		public bool CanInteractWith(PlayerProximityInteractable interactable)
		{
			if (isInteracting && currentInteractable != interactable)
				return false;

			float distance =
				Vector3.SqrMagnitude(gameObject.transform.position - interactable.gameObject.transform.position);
			
			return distance <= _sqrProxDistance;
		}

		private void FixedUpdate()
		{
			if (isInteracting && !CanInteractWith(currentInteractable))
				FinishInteraction(currentInteractable);
			
			int numberOfHits = Physics.OverlapSphereNonAlloc(transform.position, _proximityDistance, _sphereCastResults, _proximityCheckLayerMask);
			if (numberOfHits > 0)
			{
				
			}
		}
	}
}