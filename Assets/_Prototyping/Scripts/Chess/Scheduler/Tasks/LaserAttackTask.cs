using System;
using System.Collections.Generic;
using _Prototyping.Chess.AttackVisuals;
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
		public bool friendlyFireEnabled;
		public LaserAttackVisuals laserVisualPrefab;

		private LaserAttackVisuals _laserVisual;
		private ChessBoardCell _stopCell;
		private List<ChessBoardCell> _targetCells;

		public LaserAttackTask(ChessManager chessManager, ChessBoard board, ChessPiece piece, ChessBoardCell cell,
			Vector2Int direction, bool stopOnHit, bool friendlyFireEnabled, LaserAttackVisuals laserVisualPrefab)
		{
			this.chessManager = chessManager;
			this.board = board;
			this.piece = piece;
			this.cell = cell;
			this.direction = direction;
			this.stopOnHit = stopOnHit;
			this.friendlyFireEnabled = friendlyFireEnabled;
			this.laserVisualPrefab = laserVisualPrefab;

			taskState = IChessScheduledTask.ScheduledTaskState.WaitingToStart;
		}
		
		public void StartTask()
		{
			taskState = IChessScheduledTask.ScheduledTaskState.Executing;
			_targetCells = new List<ChessBoardCell>();
			
			Debug.Log(JsonUtility.ToJson(this));
			
			_laserVisual = GameObject.Instantiate(laserVisualPrefab, cell.piecePositionTransform.position, cell.piecePositionTransform.rotation);
			FindTarget();
			_laserVisual.Initialize(cell, _stopCell, direction);
		}

		private void FindTarget()
		{
			Vector2Int lastCheckedPosition = cell.gridCoordinates;
			bool endPointFound = false;
			while (!endPointFound)
			{
				Vector2Int checkPosition = lastCheckedPosition + direction;
				if (!board.IsPositionOnBoard(checkPosition))
				{
					endPointFound = true;
				}
				else
				{
					ChessBoardCell checkCell = board.cells[checkPosition];
					if (!checkCell.isEmpty && stopOnHit)
					{
						_targetCells.Add(checkCell);
						if (stopOnHit)
						{
							endPointFound = true;
							_stopCell = checkCell;
						}
					}	
				}
				lastCheckedPosition = checkPosition;
			}
		}

		public void UpdateTask()
		{
			if (_laserVisual.elapsedDurationNormalized >= 1f)
			{
				ExecuteLaserAttackLogic();
			}
		}

		private void ExecuteLaserAttackLogic()
		{
			foreach (ChessBoardCell targetCell in _targetCells)
			{
				// Do not kill allied pieces if friendly fire is disabled
				if (!friendlyFireEnabled && !board.ArePiecesOnDifferentTeams(piece, targetCell.chessPiece))
					continue;
					
				board.KillPieceOnCell(targetCell, piece, false);
			}

			taskState = IChessScheduledTask.ScheduledTaskState.Finished;
		}

		public void EndTask()
		{
			if (_laserVisual != null)
				GameObject.Destroy(_laserVisual.gameObject);
		}
	}
}