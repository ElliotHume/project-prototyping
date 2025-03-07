using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	public abstract class ChessMovementType : ScriptableObject
	{
		public enum MovementType {
			None = 0,
			Up = 1,
			Down = 2,
			Right = 3,
			Left = 4,
			UpRightDiagonal = 5,
			UpLeftDiagonal = 6,
			DownRightDiagonal = 7,
			DownLeftDiagonal = 8,
			LShape = 9,
			Castle = 10,
		}

		public enum MovementAttackType
		{
			Default,
			AttackOnly,
			MovementOnly,
		}

		public int range = 8;
		public MovementAttackType movementAttackType;

		public abstract MovementType movementType { get; }
		
		public abstract Vector2Int GetNextPosition(Vector2Int originLocation, int distance);
		
		public virtual List<Vector2Int> GetPossibleMovePositions(ChessPiece piece, ChessBoard board)
		{
			Vector2Int position;
			List<Vector2Int> positions = new List<Vector2Int>();
			for (int i = 1; i <= range; i++)
			{
				position = GetNextPosition(piece.gridCoordinates, i);

				(bool isPossiblePosition, bool canContinuePast) resultsCheck =
					movementAttackType == MovementAttackType.AttackOnly
						? CheckOffBoardAndPieceTakeAttack(position, piece, board)
						: movementAttackType == MovementAttackType.MovementOnly
								? CheckOffBoardNoAttack(position, piece, board)
								: CheckOffBoardAndPieceTake(position, piece, board);
				
				if (resultsCheck.isPossiblePosition)
					positions.Add(position);

				if (!resultsCheck.canContinuePast)
					break;
			}

			return positions;
		}
		
		protected virtual (bool, bool) CheckOffBoardAndPieceTake(Vector2Int position, ChessPiece piece, ChessBoard board)
		{
			bool isPossiblePosition = true;
			bool canContinuePast = true;
			// If the position is off of the board, early return
			if (!board.IsPositionOnBoard(position))
				return (false, false);

			if (!board.cells[position].isEmpty)
			{
				canContinuePast = false;
				if (!board.ArePiecesOnDifferentTeams(piece, board.cells[position].chessPiece))
					isPossiblePosition = false;
			}

			return (isPossiblePosition, canContinuePast);
		}
		
		/// <summary>
		/// Checks for valid positions ONLY where pieces can be taken on the board, cannot be used to move to empty spaces
		/// </summary>
		protected virtual (bool, bool) CheckOffBoardAndPieceTakeAttack(Vector2Int position, ChessPiece piece, ChessBoard board)
		{
			bool isPossiblePosition = false;
			bool canContinuePast = board.IsPositionOnBoard(position);

			if (canContinuePast && !board.cells[position].isEmpty)
			{
				canContinuePast = false;
				if (board.ArePiecesOnDifferentTeams(piece, board.cells[position].chessPiece))
					isPossiblePosition = true;
			}

			return (isPossiblePosition, canContinuePast);
		}
		
		protected virtual (bool, bool) CheckOffBoardNoAttack(Vector2Int position, ChessPiece piece, ChessBoard board)
		{
			bool isPossiblePosition = true;
			bool canContinuePast = true;
			// If the position is off of the board, early return
			if (!board.IsPositionOnBoard(position))
				return (false, false);

			if (!board.cells[position].isEmpty)
			{
				canContinuePast = false;
				// TODO: Castling
				isPossiblePosition = false;
			}

			return (isPossiblePosition, canContinuePast);
		}
	}
}