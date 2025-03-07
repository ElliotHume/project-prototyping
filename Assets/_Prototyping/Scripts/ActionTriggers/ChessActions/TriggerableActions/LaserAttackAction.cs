using System;
using _Prototyping.ActionTriggers.ChessActions.Interfaces;
using _Prototyping.ActionTriggers.Core;
using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions.TriggerableActions
{
	public class LaserAttackAction : ScriptableObject, IChessTriggerableAction
	{
		public Action<IActionTrigger<ChessActionData>> OnActionTriggered { get; set; }
		public void Trigger(ChessActionData triggerData)
		{
			Debug.Log("TRIGGERED A LASER ATTACK");
		}
	}
}