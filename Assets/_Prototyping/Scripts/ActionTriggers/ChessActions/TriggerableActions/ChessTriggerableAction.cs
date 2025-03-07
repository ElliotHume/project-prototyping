using System;
using _Prototyping.ActionTriggers.ChessActions.Interfaces;
using _Prototyping.ActionTriggers.Core;
using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions.TriggerableActions
{
	public abstract class ChessTriggerableAction : ScriptableObject, IChessTriggerableAction
	{
		public Action<ChessActionData> OnActionTriggered { get; set; }
		public abstract void Trigger(ChessActionData triggerData);
	}
}