using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(LShapeMovementType), fileName = nameof(LShapeMovementType)+"_Default")]
	public class LShapeMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.LShape;
		
		public override List<Vector2Int> GetPossibleMovePositions(Vector2Int currentPosition)
		{
			List<Vector2Int> positions = new List<Vector2Int>();
			
			positions.Add(new Vector2Int(currentPosition.x+1, currentPosition.y+2));
			positions.Add(new Vector2Int(currentPosition.x+1, currentPosition.y-2));
			positions.Add(new Vector2Int(currentPosition.x-1, currentPosition.y+2));
			positions.Add(new Vector2Int(currentPosition.x-1, currentPosition.y-2));
			
			positions.Add(new Vector2Int(currentPosition.x+2, currentPosition.y+1));
			positions.Add(new Vector2Int(currentPosition.x+2, currentPosition.y-1));
			positions.Add(new Vector2Int(currentPosition.x-2, currentPosition.y+1));
			positions.Add(new Vector2Int(currentPosition.x-2, currentPosition.y-1));
			
			return positions;
		}
	}
}