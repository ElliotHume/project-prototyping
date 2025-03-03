using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(UpRightDiagonalMovementType), fileName = nameof(UpRightDiagonalMovementType))]
	public class UpRightDiagonalMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.UpRightDiagonal;
		
		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x+distance, originLocation.y+distance);
	}
}