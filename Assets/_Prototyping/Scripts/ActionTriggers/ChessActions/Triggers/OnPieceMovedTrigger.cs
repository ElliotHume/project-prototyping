using System.Collections.Generic;
using _Prototyping.Chess;
using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions.Triggers
{
	[CreateAssetMenu(fileName = "OnPieceMovedTrigger", menuName = "PROTO/Chess/Triggers/OnPieceMovedTrigger")]
	public class OnPieceMovedTrigger : ChessActionTrigger
	{
		private const string _triggerId = "OnPieceMovedTrigger";
		public override string triggerId => _triggerId;

		public override ChessActionTrigger InitializeInstance(ChessManager chessManager, ChessBoard chessBoard, ChessPiece chessPiece)
		{
			OnPieceMovedTrigger instance = (OnPieceMovedTrigger)base.InitializeInstance(chessManager, chessBoard, chessPiece);
			chessPiece.OnChangedCellsUnityEvent.AddListener(instance.TriggerActions);
			return instance;
		}
		
		private void TriggerActions(ChessBoardCell movedToCell)
		{
			TriggerActions( new ChessActionData()
			{
				chessManager = chessManager,
				chessBoard = chessBoard,
				piece = chessPiece,
				cell = movedToCell,
			});
		}
		
		public override void CleanUp()
		{
			chessPiece.OnChangedCellsUnityEvent.RemoveListener(TriggerActions);
			
			base.CleanUp();
		}
	}
}