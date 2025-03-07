using _Prototyping.ActionTriggers.Core;
using _Prototyping.Chess;

namespace _Prototyping.ActionTriggers.ChessActions.Interfaces
{
	public interface IChessActionTrigger : IActionTrigger<ChessActionData>
	{
		public void Initialize(ChessManager chessManager, ChessBoard chessBoard, ChessPiece piece);
		public void CleanUp();
	}
}