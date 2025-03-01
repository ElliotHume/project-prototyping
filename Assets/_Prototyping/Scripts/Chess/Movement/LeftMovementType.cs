using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(LeftMovementType), fileName = nameof(LeftMovementType)+"_Default")]
	public class LeftMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.Left;
		
		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x-distance, originLocation.y);
	}
}