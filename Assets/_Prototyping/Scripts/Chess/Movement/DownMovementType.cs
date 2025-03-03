using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(DownMovementType), fileName = nameof(DownMovementType))]
	public class DownMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.Down;
		
		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x, originLocation.y - distance);
	}
}