using System.Collections.Generic;
using _Prototyping.Chess;
using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions.Triggers
{
	[CreateAssetMenu(fileName = "OnThisPieceTakenTrigger", menuName = "PROTO/Chess/Triggers/OnThisPieceTakenTrigger")]
	public class OnThisPieceTakenTrigger : ChessActionTrigger
	{
		private const string _triggerId = "OnThisPieceTakenTrigger";
		public override string triggerId => _triggerId;
		
		public override ChessActionTrigger InitializeInstance(ChessManager chessManager, ChessBoard chessBoard, ChessPiece chessPiece)
		{
			OnThisPieceTakenTrigger instance = (OnThisPieceTakenTrigger)base.InitializeInstance(chessManager, chessBoard, chessPiece);
			chessPiece.OnThisPieceTaken += instance.TriggerActions;
			return instance;
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