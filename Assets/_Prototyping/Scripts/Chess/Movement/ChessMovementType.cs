using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	public abstract class ChessMovementType : ScriptableObject
	{
		public enum MovementType
		{
			None = 0,
			Forward = 1,
			Backward = 2,
			Right = 3,
			Left = 4,
			ForwardRightDiagonal = 5,
			ForwardLeftDiagonal = 6,
			BackwardRightDiagonal = 7,
			BackwardLeftDiagonal = 8,
			LShape = 9,
			Castle = 10,
		}

		public int range = 8;

		public abstract MovementType movementType { get; }
		public abstract List<Vector2Int> GetPossibleMovePositions(Vector2Int currentPosition);
	}
}