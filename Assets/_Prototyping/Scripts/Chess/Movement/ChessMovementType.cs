using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	public abstract class ChessMovementType : ScriptableObject
	{
		public enum MovementType
		{
			None = 0,
			Forward = 1,
			Backward = 2,
			Right = 3,
			Left = 4,
			ForwardRightDiagonal = 5,
			ForwardLeftDiagonal = 6,
			BackwardRightDiagonal = 7,
			BackwardLeftDiagonal = 8,
			LShape = 9,
			Castle = 10,
		}

		public int range = 8;

		public abstract MovementType movementType { get; }
		
		public abstract Vector2Int GetNextPosition(Vector2Int originLocation, int distance);
		
		public virtual List<Vector2Int> GetPossibleMovePositions(ChessPiece piece, ChessBoard board)
		{
			Vector2Int position;
			List<Vector2Int> positions = new List<Vector2Int>();
			for (int i = 0; i < range; i++)
			{
				position = GetNextPosition(piece.gridCoordinates, i);

				(bool isPossiblePosition, bool canContinuePast) resultsCheck = CheckOffBoardAndPieceTake(position, piece, board);
				
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
			// If the position if off of the board
			if (!board.IsPositionOnBoard(position))
			{
				isPossiblePosition = false;
				canContinuePast = false;
			}
				
			if (!board.cells[position].isEmpty)
			{
				canContinuePast = false;
				if (!board.CanPieceTakeOther(piece, board.cells[position].chessPiece))
					isPossiblePosition = false;
			}

			return (isPossiblePosition, canContinuePast);
		}
	}
}