using _Prototyping.ActionTriggers.Core;
using _Prototyping.Chess;

namespace _Prototyping.ActionTriggers.ChessActions.Interfaces
{
	public interface IChessActionTrigger<T> : IActionTrigger<ChessActionData>
	{
		public T InitializeInstance(ChessManager chessManager, ChessBoard chessBoard, ChessPiece piece);
		public void CleanUp();
	}
}