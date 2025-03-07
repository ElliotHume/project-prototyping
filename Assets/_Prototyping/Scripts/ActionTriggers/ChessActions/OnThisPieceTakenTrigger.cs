using System;
using System.Collections.Generic;
using _Prototyping.ActionTriggers.ChessActions.Interfaces;
using _Prototyping.ActionTriggers.Core;
using _Prototyping.Chess;
using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions
{
	public class OnThisPieceTakenTrigger : ScriptableObject, IChessActionTrigger
	{
		public List<ITriggerableAction<ChessActionData>> triggerables { get; private set; }
		public Action<ChessActionData> OnTriggered { get; set; }

		public bool isInitialized;
		
		private ChessManager _chessManager;
		private ChessBoard _chessBoard;
		private ChessPiece _chessPiece;

		private ChessPiece _takenByPiece;
		
		public void Initialize(ChessManager chessManager, ChessBoard chessBoard, ChessPiece chessPiece)
		{
			_chessManager = chessManager;
			_chessBoard = chessBoard;
			_chessPiece = chessPiece;

			_chessPiece.OnThisPieceTaken += TriggerActions;
			isInitialized = true;
		}
		
		public void AddAction(ITriggerableAction<ChessActionData> triggerable)
		{
			if (!triggerables.Contains(triggerable))
			{
				triggerables.Add(triggerable);
				OnTriggered += triggerable.Trigger;
			}
		}

		public void RemoveAction(ITriggerableAction<ChessActionData> triggerable)
		{
			if (triggerables.Contains(triggerable))
			{
				OnTriggered -= triggerable.Trigger;
				triggerables.Remove(triggerable);
			}
		}

		private void TriggerActions(ChessPiece pieceTakingThisOne)
		{
			TriggerActions( new ChessActionData()
			{
				chessManager = _chessManager,
				chessBoard = _chessBoard,
				triggeredPiece = _chessPiece,
				
				triggeringPiece = pieceTakingThisOne,
			});
		}

		public void TriggerActions(ChessActionData triggerData)
		{
			OnTriggered?.Invoke(triggerData);
		}

		public void CleanUp()
		{
			if (!isInitialized)
				return;
			
			_chessPiece.OnThisPieceTaken -= TriggerActions;
			
			foreach (ITriggerableAction<ChessActionData> triggerableAction in triggerables)
				RemoveAction(triggerableAction);
		}
	}
}