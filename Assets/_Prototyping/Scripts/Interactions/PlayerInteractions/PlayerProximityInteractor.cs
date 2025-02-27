using System;
using _Prototyping.Interactions.Core;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;

namespace _Prototyping.Interactions.PlayerInteractions
{
	public class PlayerProximityInteractor : MonoBehaviour, IInteractor<PlayerProximityInteractable>
	{
		#region IInteractor

		public PlayerProximityInteractable currentInteractable { get; private set; }
		PlayerProximityInteractable IInteractor<PlayerProximityInteractable>.CurrentInteractable => currentInteractable;

		public PlayerProximityInteractable hoveredInteractable { get; private set; }
		PlayerProximityInteractable IInteractor<PlayerProximityInteractable>.HoveredInteractable => hoveredInteractable;
		
		public Action<PlayerProximityInteractable> OnStartInteraction { get; set; }
		public Action<PlayerProximityInteractable> OnEndInteraction { get; set; }
		public Action<PlayerProximityInteractable> OnStartHover { get; set; }
		public Action<PlayerProximityInteractable> OnEndHover { get; set; }

		#endregion

		[SerializeField]
		private float _proximityDistance = 1f;

		[SerializeField]
		private LayerMask _proximityCheckLayerMask;

		public bool isInteracting => currentInteractable != null;
		public bool isHovering => hoveredInteractable != null;
		public bool isLowPriority => false;
		
		private float _sqrProxDistance;
		private Collider[] _sphereCastResults;

		private void Awake()
		{
			_sqrProxDistance = _proximityDistance * _proximityDistance;
			_sphereCastResults = new Collider[5];
		}

		private void SetHover(PlayerProximityInteractable interactable)
		{
			if (hoveredInteractable != interactable)
			{
				if (hoveredInteractable != null)
					EndHover(hoveredInteractable);

				hoveredInteractable = interactable;
				
				if (hoveredInteractable != null)
					StartHover(hoveredInteractable);
			}
		}

		public void StartInteraction(PlayerProximityInteractable interactable)
		{
			OnStartInteraction?.Invoke(interactable);
			currentInteractable = interactable;
			currentInteractable.StartInteraction(this);
		}

		public void EndInteraction(PlayerProximityInteractable interactable)
		{
			OnEndInteraction?.Invoke(interactable);
			currentInteractable.EndInteraction(this);
			
			currentInteractable = null;
		}

		public void StartHover(PlayerProximityInteractable interactable)
		{
			OnStartHover?.Invoke(interactable);
		}

		public void EndHover(PlayerProximityInteractable interactable)
		{
			OnEndHover?.Invoke(interactable);
		}

		public bool CanInteractWith(PlayerProximityInteractable interactable)
		{
			if (isInteracting && currentInteractable != interactable)
			{
				return false;
			}
				

			float distance =
				Vector3.SqrMagnitude(transform.position - interactable.collider.bounds.center);

			return distance <= _sqrProxDistance;
		}

		private void FixedUpdate()
		{
			if (isInteracting)
			{
				if (!CanInteractWith(currentInteractable))
				{
					EndInteraction(currentInteractable);
				}
			}
			else
			{
				ProcessHovers();
			}
		}
		
		public bool OnInteractInputPressed()
		{
			if (isInteracting)
			{
				EndInteraction(currentInteractable);
				return true;
			}
			else
			{
				if (isHovering)
				{
					StartInteraction(hoveredInteractable);
					return true;
				}
			}

			return false;
		}

		private void ProcessHovers()
		{
			int numberOfHits = Physics.OverlapSphereNonAlloc(transform.position, _proximityDistance, _sphereCastResults, _proximityCheckLayerMask, QueryTriggerInteraction.Collide);
			float closestColliderDistance = _proximityDistance;
			PlayerProximityInteractable closestInteractable = null;
			for (int i = 0; i < numberOfHits; i++)
			{
				Collider hitCollider = _sphereCastResults[i];
				float sqrDistanceToCollider = Vector3.SqrMagnitude(hitCollider.bounds.center - transform.position);
				PlayerProximityInteractable playerProximityInteractable =
					hitCollider.gameObject.GetComponentInChildren<PlayerProximityInteractable>();
				
				if (sqrDistanceToCollider < closestColliderDistance
					&& playerProximityInteractable != null
					&& CanInteractWith(playerProximityInteractable))
				{
					closestColliderDistance = sqrDistanceToCollider;
					closestInteractable = playerProximityInteractable;
				}
			}
			SetHover(closestInteractable);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.gray;
			Gizmos.DrawWireSphere(transform.position, _proximityDistance);
		}
	}
}