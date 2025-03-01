using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(BackwardMovementType), fileName = nameof(BackwardMovementType)+"_Default")]
	public class BackwardMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.Backward;
		
		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x, originLocation.y - distance);
	}
}