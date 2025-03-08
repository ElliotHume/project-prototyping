using _Prototyping.Chess.AttackVisuals;
using _Prototyping.Chess.Scheduler;
using _Prototyping.Chess.Scheduler.Tasks;
using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions.TriggerableActions
{
	[CreateAssetMenu(fileName = "LaserAttackAction", menuName = "PROTO/Chess/TriggerableActions/LaserAttackAction")]
	public class LaserAttackAction : ChessTriggerableAction
	{
		public Vector2Int direction;
		public bool stopOnHit;
		public bool friendlyFireEnabled;
		public LaserAttackVisuals laserAttackVisualsPrefab;
		
		public override void Trigger(ChessActionData triggerData)
		{
			Debug.Log($"Firing a laser at cell [{triggerData.cell.gridCoordinates}]-pos:[{triggerData.cell.piecePositionTransform.position}] with direction [{direction}]", triggerData.cell);
			
			ChessScheduler.Instance.AddTaskToCurrentSchedule(
				new LaserAttackTask(triggerData.chessManager, triggerData.chessBoard, triggerData.piece, triggerData.cell, direction, stopOnHit, friendlyFireEnabled, laserAttackVisualsPrefab));
			OnActionTriggered?.Invoke(triggerData);
		}
	}
}