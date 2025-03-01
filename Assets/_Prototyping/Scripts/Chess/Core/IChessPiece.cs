using System.Collections.Generic;
using _Prototyping.Chess.Movement;

namespace _Prototyping.Chess.Core
{
	public interface IChessPiece
	{
		public ChessPieceType type { get; }
		public List<ChessMovementType> movementOptions { get; set; }
		public bool isPlayerControlled { get; }
		public ChessBoardCell cell { get; }
	}
}