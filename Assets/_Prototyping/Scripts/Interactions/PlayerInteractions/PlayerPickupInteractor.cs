using System;
using System.Collections.Generic;
using System.Linq;
using _Prototyping.Interactions.Core;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;

namespace _Prototyping.Interactions.PlayerInteractions
{
	public class PlayerPickupInteractor : MonoBehaviour, IInteractor<PlayerPickupInteractable>, IPlayerInteractInputReceiver
	{
		#region IInteractor

		public PlayerPickupInteractable currentInteractable { get; private set; }
		PlayerPickupInteractable IInteractor<PlayerPickupInteractable>.CurrentInteractable => currentInteractable;

		public PlayerPickupInteractable hoveredInteractable { get; private set; }
		PlayerPickupInteractable IInteractor<PlayerPickupInteractable>.HoveredInteractable => hoveredInteractable;
		
		public Action<PlayerPickupInteractable> OnStartInteraction { get; set; }
		public Action<PlayerPickupInteractable> OnEndInteraction { get; set; }
		public Action<PlayerPickupInteractable> OnStartHover { get; set; }
		public Action<PlayerPickupInteractable> OnEndHover { get; set; }

		#endregion
		

		[SerializeField]
		private List<Transform> _pickupTransformPositions;

		[SerializeField]
		private Transform _dropPointTransform;

		[SerializeField]
		private float _proximityStartDistance = 1f;
		
		[SerializeField]
		private float _proximityStopDistance = 2f;

		[SerializeField, Tooltip("How strongly are objects held, the lower the value, the more the object will lag behind the hold point")]
		private float _holdForce = 100f;

		[SerializeField]
		private LayerMask _proximityCheckLayerMask;

		public List<PlayerPickupInteractable> pickedUpObjects { get; private set; }
		public int pickupCapacity => _pickupTransformPositions.Count;
		
		public bool isInteracting => pickedUpObjects.Count > 0;
		public bool isHovering => hoveredInteractable != null;
		public int priority => isInteracting && !isHovering ? 1 : 0;

		private float _sqrProxStartDistance;
		private float _sqrProxStopDistance;
		private Collider[] _sphereCastResults;

		private void Awake()
		{
			pickedUpObjects = new List<PlayerPickupInteractable>();
			
			_sqrProxStartDistance = _proximityStartDistance * _proximityStartDistance;
			_sqrProxStopDistance = _proximityStopDistance * _proximityStopDistance;
			_sphereCastResults = new Collider[5];
		}

		private void SetHover(PlayerPickupInteractable interactable)
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

		public void StartInteraction(PlayerPickupInteractable interactable)
		{
			if (interactable == null || !CanInteractWith(interactable))
			{
				Debug.LogError($"[{nameof(PlayerPickupInteractor)}] Tried to start an interaction with an interactable that is null or cant be interacted with.", interactable);
				return;
			}

			OnStartInteraction?.Invoke(interactable);
			currentInteractable = interactable;
			currentInteractable.StartInteraction(this);

			if (interactable != null && !pickedUpObjects.Contains(interactable))
			{
				pickedUpObjects.Add(interactable);
				
				EndHover(interactable);
				
				// Teleport the object to the corresponding pickup point transform
				Transform pickupPoint = _pickupTransformPositions[pickedUpObjects.IndexOf(interactable)];
				interactable.rootGameObject.transform.SetPositionAndRotation(pickupPoint.position, pickupPoint.rotation);
			}
				
		}

		public void EndInteraction(PlayerPickupInteractable interactable)
		{
			if (interactable == null)
			{
				Debug.LogError($"[{nameof(PlayerPickupInteractor)}] Tried to end an interaction with an interactable that is null");
				return;
			}
			
			OnEndInteraction?.Invoke(interactable);
			interactable.EndInteraction(this);

			if (interactable != null && pickedUpObjects.Contains(interactable))
			{
				// Teleport the object to the drop point if its within acceptable range
				if (CanInteractWith(interactable))
					interactable.rootGameObject.transform.SetPositionAndRotation(_dropPointTransform.position, _dropPointTransform.rotation);
				
				pickedUpObjects.Remove(interactable);
			}
				
			
			currentInteractable = pickedUpObjects.Count > 0 ? pickedUpObjects.Last() : null;
		}

		public void StartHover(PlayerPickupInteractable interactable)
		{
			OnStartHover?.Invoke(interactable);
		}

		public void EndHover(PlayerPickupInteractable interactable)
		{
			OnEndHover?.Invoke(interactable);
		}

		public bool CanInteractWith(PlayerPickupInteractable interactable)
		{
			bool isPickedUp = pickedUpObjects.Contains(interactable);
			if (!isPickedUp && pickedUpObjects.Count >= pickupCapacity)
			{
				return false;
			}

			float range = isPickedUp ? _sqrProxStopDistance : _sqrProxStartDistance;
			bool distanceCheck = Vector3.SqrMagnitude(transform.position - interactable.collider.bounds.center) <= range;

			return distanceCheck;
		}

		private void Update()
		{
			ProcessHovers();
			
			if (isInteracting)
			{
				for (int i = 0; i < pickedUpObjects.Count; i++)
				{
					PlayerPickupInteractable pickedUpObject = pickedUpObjects[i];
					if (!CanInteractWith(pickedUpObject))
					{
						EndInteraction(pickedUpObject);
					}

					Transform pickupPoint = _pickupTransformPositions[i];
					pickedUpObject.rootGameObject.transform.position = Vector3.Lerp(
						pickedUpObject.rootGameObject.transform.position, pickupPoint.position,
						Time.deltaTime * _holdForce);
					pickedUpObject.rootGameObject.transform.rotation = Quaternion.Lerp(
						pickedUpObject.rootGameObject.transform.rotation, pickupPoint.rotation,
						Time.deltaTime * _holdForce);
				}
			}
		}
		
		public bool OnInteractInputPressed()
		{
			if (hoveredInteractable != null)
			{
				StartInteraction(hoveredInteractable);
				return true;
			}
			else
			{
				if (pickedUpObjects.Count > 0)
				{
					EndInteraction(pickedUpObjects.Last());
					return true;
				}
			}

			return false;
		}

		public bool OnInteractInputHeld(float heldDuration)
		{
			// TODO:
			return false;
		}

		public void OnInteractInputReleased()
		{
			// TODO:
		}

		private void ProcessHovers()
		{
			int numberOfHits = Physics.OverlapSphereNonAlloc(transform.position, _proximityStartDistance, _sphereCastResults, _proximityCheckLayerMask, QueryTriggerInteraction.Collide);
			float closestColliderDistance = _proximityStartDistance;
			PlayerPickupInteractable closestInteractable = null;
			for (int i = 0; i < numberOfHits; i++)
			{
				Collider hitCollider = _sphereCastResults[i];
				float sqrDistanceToCollider = Vector3.SqrMagnitude(hitCollider.bounds.center - transform.position);
				PlayerPickupInteractable playerPickupInteractable =
					hitCollider.gameObject.GetComponentInChildren<PlayerPickupInteractable>();

				if (sqrDistanceToCollider < closestColliderDistance
					&& playerPickupInteractable != null
					&& playerPickupInteractable.currentInteractor == null // Only process hovers on objects that arent already picked up
					&& CanInteractWith(playerPickupInteractable))
				{
					closestColliderDistance = sqrDistanceToCollider;
					closestInteractable = playerPickupInteractable;
				}
			}
			SetHover(closestInteractable);
		}

		private void OnDrawGizmosSelected()
		{
			Gizmos.color = Color.gray;
			Gizmos.DrawWireSphere(transform.position, _proximityStartDistance);
			Gizmos.color = Color.black;
			Gizmos.DrawWireSphere(transform.position, _proximityStopDistance);

			Gizmos.color = Color.blue;
			foreach (Transform pickupTransform in _pickupTransformPositions)
			{
				Gizmos.DrawWireSphere(pickupTransform.position, 0.1f);
			}
		}
	}
}