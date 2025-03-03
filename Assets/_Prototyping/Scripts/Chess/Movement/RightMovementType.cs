using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(RightMovementType), fileName = nameof(RightMovementType))]
	public class RightMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.Right;
		
		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x+distance, originLocation.y);
	}
}