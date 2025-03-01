using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(BackwardRightDiagonalMovementType), fileName = nameof(BackwardRightDiagonalMovementType) + "_Default")]
	public class BackwardRightDiagonalMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.BackwardRightDiagonal;
		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x+distance, originLocation.y-distance);
	}
}