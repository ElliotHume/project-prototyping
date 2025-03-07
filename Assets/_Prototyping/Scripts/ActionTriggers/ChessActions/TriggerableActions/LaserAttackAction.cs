using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions.TriggerableActions
{
	[CreateAssetMenu(fileName = "LaserAttackAction", menuName = "PROTO/Chess/TriggerableActions/LaserAttackAction")]
	public class LaserAttackAction : ChessTriggerableAction
	{
		public override void Trigger(ChessActionData triggerData)
		{
			Debug.Log("TRIGGERED A LASER ATTACK");
			OnActionTriggered?.Invoke(triggerData);
		}
	}
}