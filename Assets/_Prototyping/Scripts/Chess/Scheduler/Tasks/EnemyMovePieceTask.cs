using System;

namespace _Prototyping.Chess.Scheduler.Tasks
{
	public class EnemyMovePieceTask : IChessScheduledTask
	{
		public ChessManager.ChessGameState scheduledTurn { get; }

		public IChessScheduledTask.ScheduledTaskState taskState { get; private set; } =
			IChessScheduledTask.ScheduledTaskState.WaitingToStart;
		public Action<IChessScheduledTask.ScheduledTaskState> OnStateChanged { get; set; }

		public ChessManager chessManager;
		public ChessBoard board;
		public ChessPiece piece;
		public ChessBoardCell cell;

		public EnemyMovePieceTask(ChessManager chessManager, ChessBoard board, ChessPiece piece, ChessBoardCell cell)
		{
			this.chessManager = chessManager;
			this.board = board;
			this.piece = piece;
			this.cell = cell;

			taskState = IChessScheduledTask.ScheduledTaskState.WaitingToStart;
		}
		
		public void StartTask()
		{
			// TODO: Add animations
			taskState = IChessScheduledTask.ScheduledTaskState.Executing;
		}

		public void UpdateTask()
		{
			if (!cell.isEmpty && cell.chessPiece.isPlayerControlled)
			{
				chessManager.OnPlayerPieceTakenUnityEvent?.Invoke(cell.chessPiece);
				board.KillPieceOnCell(cell, piece, true);
			}
			piece.MoveToCell(cell);
			
			taskState = IChessScheduledTask.ScheduledTaskState.Finished;
		}

		public void EndTask()
		{
			// Do nothing, nothing to do
		}
	}
}