using UnityEngine;

namespace _Prototyping.Chess.Movement
{
	[CreateAssetMenu(menuName = "PROTO/Chess/Movement/" + nameof(ForwardMovementType), fileName = nameof(ForwardMovementType)+"_Default")]
	public class ForwardMovementType : ChessMovementType
	{
		public override MovementType movementType => MovementType.Forward;

		public override Vector2Int GetNextPosition(Vector2Int originLocation, int distance) => new Vector2Int(originLocation.x, originLocation.y + distance);
	}
}