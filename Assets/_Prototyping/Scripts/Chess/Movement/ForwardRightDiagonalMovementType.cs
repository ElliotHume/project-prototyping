using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(ForwardRightDiagonalMovementType), fileName = nameof(ForwardRightDiagonalMovementType) + "_Default")]
	public class ForwardRightDiagonalMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.ForwardRightDiagonal;
		
		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x+distance, originLocation.y+distance);
	}
}