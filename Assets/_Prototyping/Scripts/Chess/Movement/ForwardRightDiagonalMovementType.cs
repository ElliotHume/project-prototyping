using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(ForwardRightDiagonalMovementType), fileName = nameof(ForwardRightDiagonalMovementType) + "_Default")]
	public class ForwardRightDiagonalMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.ForwardRightDiagonal;

		public override List<Vector2Int> GetPossibleMovePositions(Vector2Int currentPosition)
		{
			List<Vector2Int> positions = new List<Vector2Int>();
			for (int i = 0; i < range; i++)
			{
				positions.Add(new Vector2Int(currentPosition.x+i, currentPosition.y+i));
			}

			return positions;
		}
	}
}