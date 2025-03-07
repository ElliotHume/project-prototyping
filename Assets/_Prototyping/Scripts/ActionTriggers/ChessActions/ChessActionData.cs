using System.Collections.Generic;
using _Prototyping.Chess;

namespace _Prototyping.ActionTriggers.ChessActions
{
	public struct ChessActionData
	{
		public ChessManager chessManager;
		public ChessBoard chessBoard;
		public ChessPiece piece;
		public ChessBoardCell cell;
		
		public List<ChessPiece> paramPieces;
		public List<ChessBoardCell> paramCells;
	}
}