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
			_inputReader.onInteracted += OnInteracted;
		}

		private void OnInteracted()
		{
			foreach (SerializableUnityRef<IBaseInteractor> interactor in _interactorsInPriorityOrder.OrderBy((i) => i.value.priority))
			{
				if (interactor.value.OnInteractInputPressed())
					return;
			}
		}
	}
}