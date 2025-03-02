using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(ForwardDiagonalAttackMovementType), fileName = nameof(ForwardDiagonalAttackMovementType)+"_Default")]
	public class ForwardDiagonalAttackMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.Forward;

		// UNUSED
		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int();
		
		// TODO: Refactor to use range and be separated per axis
		public override List<Vector2Int> GetPossibleMovePositions(ChessPiece piece, ChessBoard board)
		{
			List<Vector2Int> positions = new List<Vector2Int>();
			
			positions.Add(new Vector2Int(piece.x+1, piece.y+1));
			positions.Add(new Vector2Int(piece.x-1, piece.y+1));

			foreach (Vector2Int position in positions.ToArray())
			{
				(bool isPossiblePosition, bool canContinuePast) resultsCheck = CheckOffBoardAndPieceTakeAttack(position, piece, board);
				
				if (!resultsCheck.isPossiblePosition)
					positions.Remove(position);
			}

			return positions;
		}
	}
}