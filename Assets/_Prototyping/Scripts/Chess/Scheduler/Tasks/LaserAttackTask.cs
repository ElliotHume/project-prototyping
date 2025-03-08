using System;
using UnityEngine;

namespace _Prototyping.Chess.Scheduler.Tasks
{
	public class LaserAttackTask : IChessScheduledTask
	{
		public ChessManager.ChessGameState scheduledTurn { get; }

		public IChessScheduledTask.ScheduledTaskState taskState { get; private set; } =
			IChessScheduledTask.ScheduledTaskState.WaitingToStart;
		public Action<IChessScheduledTask.ScheduledTaskState> OnStateChanged { get; set; }

		public ChessManager chessManager;
		public ChessBoard board;
		public ChessPiece piece;
		public ChessBoardCell cell;

		public Vector2Int direction;
		public bool stopOnHit;
		public GameObject laserVisualPrefab;
		public float speed;

		private GameObject _laserVisual;

		public LaserAttackTask(ChessManager chessManager, ChessBoard board, ChessPiece piece, ChessBoardCell cell,
			Vector2Int direction, bool stopOnHit, GameObject laserVisualPrefab, float speed)
		{
			this.chessManager = chessManager;
			this.board = board;
			this.piece = piece;
			this.cell = cell;
			this.direction = direction;
			this.stopOnHit = stopOnHit;
			this.laserVisualPrefab = laserVisualPrefab;
			this.speed = speed;

			taskState = IChessScheduledTask.ScheduledTaskState.WaitingToStart;
		}
		
		public void StartTask()
		{
			_laserVisual = GameObject.Instantiate(laserVisualPrefab, cell.piecePositionTransform.position, cell.piecePositionTransform.rotation);
			taskState = IChessScheduledTask.ScheduledTaskState.Executing;
		}

		public void UpdateTask()
		{
			
			
		}

		public void EndTask()
		{
			// Do nothing, nothing to do
		}
	}
}