using _Prototyping.ActionTriggers.Core;
using _Prototyping.Chess;

namespace _Prototyping.ActionTriggers.ChessActions.Interfaces
{
	public interface IChessActionTrigger<T> : IActionTrigger<ChessActionData>
	{
		/// <summary>
		/// Set up a new instance of the trigger.
		/// </summary>
		/// <param name="chessManager"></param>
		/// <param name="chessBoard"></param>
		/// <param name="piece"></param>
		/// <returns>The newly instantiated instance</returns>
		public T InitializeInstance(ChessManager chessManager, ChessBoard chessBoard, ChessPiece piece);
		
		/// <summary>
		/// Clean up the instance, removing any listeners, unsubscribing from events, etc...
		/// </summary>
		public void CleanUp();
	}
}