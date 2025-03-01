using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(ForwardLeftDiagonalMovementType), fileName = nameof(ForwardLeftDiagonalMovementType) + "_Default")]
	public class ForwardLeftDiagonalMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.ForwardLeftDiagonal;
		
		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x-distance, originLocation.y+distance);
	}
}