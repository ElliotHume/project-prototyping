using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(BackwardLeftDiagonalMovementType), fileName = nameof(BackwardLeftDiagonalMovementType) + "_Default")]
	public class BackwardLeftDiagonalMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.BackwardLeftDiagonal;
		
		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x-distance, originLocation.y-distance);
	}
}