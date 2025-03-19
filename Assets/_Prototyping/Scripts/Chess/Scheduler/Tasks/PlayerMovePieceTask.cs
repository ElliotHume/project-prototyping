using System;
using _Prototyping.Chess.Visuals;
using UnityEngine;

namespace _Prototyping.Chess.Scheduler.Tasks
{
	public class PlayerMovePieceTask : IChessScheduledTask
	{
		public const string PATH_TO_RESOURCE = "Prefabs/ChessMoveVisual";

		public ChessManager.ChessGameState scheduledTurn { get; }

		public IChessScheduledTask.ScheduledTaskState taskState { get; private set; }
		public Action<IChessScheduledTask.ScheduledTaskState> OnStateChanged { get; set; }

		public ChessManager chessManager;
		public ChessBoard board;
		public ChessPiece piece;
		public ChessBoardCell cell;
		
		private ChessMoveVisual _moveVisual;

		public PlayerMovePieceTask(ChessManager chessManager, ChessBoard board, ChessPiece piece, ChessBoardCell cell)
		{
			taskState = IChessScheduledTask.ScheduledTaskState.WaitingToStart;
			
			this.chessManager = chessManager;
			this.board = board;
			this.piece = piece;
			this.cell = cell;

			// Initialize a move visual
			ChessMoveVisual moveVisualResourcePrefab = Resources.Load<ChessMoveVisual>(PATH_TO_RESOURCE);
			this._moveVisual = GameObject.Instantiate(moveVisualResourcePrefab, piece.tilePointTransform);
			this._moveVisual.Setup(piece, this);
		}
		
		public void StartTask()
		{
			// TODO: Add animations
			taskState = IChessScheduledTask.ScheduledTaskState.Executing;
		}

		public void UpdateTask()
		{
			if (!cell.isEmpty && !cell.chessPiece.isPlayerControlled)
			{
				chessManager.OnEnemyPieceTakenUnityEvent?.Invoke(cell.chessPiece);
				board.KillPieceOnCell(cell, piece);
			}

			piece.MoveToCell(cell);
			taskState = IChessScheduledTask.ScheduledTaskState.Finished;
		}

		public void EndTask()
		{
			Debug.Log("GOT HERE");
			// Clean up the move visual
			if (_moveVisual != null)
				_moveVisual.Cleanup();
		}

		public void CancelTask()
		{
			EndTask();
		}
	}
}