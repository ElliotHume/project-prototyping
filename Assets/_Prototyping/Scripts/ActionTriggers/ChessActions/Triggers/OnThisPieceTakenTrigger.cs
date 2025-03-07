using System.Collections.Generic;
using _Prototyping.Chess;
using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions.Triggers
{
	[CreateAssetMenu(fileName = "OnThisPieceTakenTrigger", menuName = "PROTO/Chess/Triggers/OnThisPieceTakenTrigger")]
	public class OnThisPieceTakenTrigger : ChessActionTrigger
	{
		public override void Initialize(ChessManager chessManager, ChessBoard chessBoard, ChessPiece chessPiece)
		{
			base.Initialize(chessManager, chessBoard, chessPiece);

			chessPiece.OnThisPieceTaken += TriggerActions;
		}
		
		private void TriggerActions(ChessPiece pieceTakingThisOne)
		{
			TriggerActions( new ChessActionData()
			{
				chessManager = chessManager,
				chessBoard = chessBoard,
				piece = chessPiece,
				
				paramPieces = new List<ChessPiece>(){pieceTakingThisOne},
			});
		}

		public override void CleanUp()
		{
			chessPiece.OnThisPieceTaken -= TriggerActions;
			
			base.CleanUp();
		}
	}
}