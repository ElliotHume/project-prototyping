using System.Collections.Generic;
using _Prototyping.Chess;
using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions.Triggers
{
	[CreateAssetMenu(fileName = "OnPieceMovedTrigger", menuName = "PROTO/Chess/Triggers/OnPieceMovedTrigger")]
	public class OnPieceMovedTrigger : ChessActionTrigger
	{
		public override void Initialize(ChessManager chessManager, ChessBoard chessBoard, ChessPiece chessPiece)
		{
			base.Initialize(chessManager, chessBoard, base.chessPiece);

			chessPiece.OnChangedCellsUnityEvent.AddListener(TriggerActions);
		}
		
		private void TriggerActions(ChessBoardCell movedToCell)
		{
			TriggerActions( new ChessActionData()
			{
				chessManager = chessManager,
				chessBoard = chessBoard,
				piece = chessPiece,
				
				paramCells = new List<ChessBoardCell>(){movedToCell},
			});
		}
		
		public override void CleanUp()
		{
			chessPiece.OnChangedCellsUnityEvent.RemoveListener(TriggerActions);
			
			base.CleanUp();
		}
	}
}