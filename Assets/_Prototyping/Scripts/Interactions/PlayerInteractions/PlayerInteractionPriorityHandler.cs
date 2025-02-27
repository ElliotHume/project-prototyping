using System.Collections.Generic;
using System.Linq;
using _Prototyping.Interactions.Core;
using _Prototyping.Utilities;
using Synty.AnimationBaseLocomotion.Samples.InputSystem;
using UnityEngine;

namespace _Prototyping.Interactions.PlayerInteractions
{
	public class PlayerInteractionPriorityHandler : MonoBehaviour
	{
		[SerializeField]
		private InputReader _inputReader;
		
		[SerializeField]
		private List<SerializableUnityRef<IBaseInteractor>> _interactorsInPriorityOrder;

		private void Start()
		{
			_inputReader.onInteractPressed += OnInteractedPressed;
			_inputReader.onInteractHeld += OnInteractHeld;
			_inputReader.onInteractReleased += OnInteractReleased;
		}

		private void OnInteractedPressed()
		{
			foreach (SerializableUnityRef<IBaseInteractor> interactor in _interactorsInPriorityOrder.OrderBy((i) => i.value.priority))
			{
				if (interactor.value is IPlayerInteractInputReceiver playerInputInteractor && playerInputInteractor.OnInteractInputPressed())
					return;
			}
		}
		
		private void OnInteractHeld(float heldDuration)
		{
			foreach (SerializableUnityRef<IBaseInteractor> interactor in _interactorsInPriorityOrder.OrderBy((i) => i.value.priority))
			{
				if (interactor.value is IPlayerInteractInputReceiver playerInputInteractor && playerInputInteractor.OnInteractInputHeld(heldDuration))
					return;
			}
		}
		
		private void OnInteractReleased()
		{
			foreach (SerializableUnityRef<IBaseInteractor> interactor in _interactorsInPriorityOrder)
			{
				if (interactor.value is IPlayerInteractInputReceiver playerInputInteractor)
					playerInputInteractor.OnInteractInputReleased();
			}
		}
	}
}