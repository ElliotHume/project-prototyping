using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(UpMovementType), fileName = nameof(UpMovementType))]
	public class UpMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.Up;

		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x, originLocation.y + distance);
	}
}