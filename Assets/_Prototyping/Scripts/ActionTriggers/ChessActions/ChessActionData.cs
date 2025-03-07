using System.Collections.Generic;
using _Prototyping.Chess;

namespace _Prototyping.ActionTriggers.ChessActions
{
	public struct ChessActionData
	{
		public ChessManager chessManager;
		public ChessBoard chessBoard;
		public ChessPiece triggeredPiece;
		public ChessPiece triggeringPiece;

		public List<ChessPiece> affectedPieces;
	}
}