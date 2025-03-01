using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(LShapeMovementType), fileName = nameof(LShapeMovementType)+"_Default")]
	public class LShapeMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.LShape;

		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance)
		{
			// Unused;
			return new Vector2Int();
		}

		public override List<Vector2Int> GetPossibleMovePositions(ChessPiece piece, ChessBoard board)
		{
			List<Vector2Int> positions = new List<Vector2Int>();
			
			positions.Add(new Vector2Int(piece.x+1, piece.y+2));
			positions.Add(new Vector2Int(piece.x+1, piece.y-2));
			positions.Add(new Vector2Int(piece.x-1, piece.y+2));
			positions.Add(new Vector2Int(piece.x-1, piece.y-2));
			
			positions.Add(new Vector2Int(piece.x+2, piece.y+1));
			positions.Add(new Vector2Int(piece.x+2, piece.y-1));
			positions.Add(new Vector2Int(piece.x-2, piece.y+1));
			positions.Add(new Vector2Int(piece.x-2, piece.y-1));

			foreach (Vector2Int position in positions.ToArray())
			{
				(bool isPossiblePosition, bool canContinuePast) resultsCheck = CheckOffBoardAndPieceTake(position, piece, board);
				
				if (!resultsCheck.isPossiblePosition)
					positions.Remove(position);
			}

			return positions;
		}
	}
}