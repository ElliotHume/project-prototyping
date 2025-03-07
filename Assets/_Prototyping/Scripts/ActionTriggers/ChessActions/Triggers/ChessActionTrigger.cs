using System;
using System.Collections.Generic;
using _Prototyping.ActionTriggers.ChessActions.Interfaces;
using _Prototyping.ActionTriggers.Core;
using _Prototyping.Chess;
using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions.Triggers
{
	public abstract class ChessActionTrigger : ScriptableObject, IChessActionTrigger
	{
		public List<ITriggerableAction<ChessActionData>> triggerables { get; private set; }
		public Action<ChessActionData> OnTriggered { get; set; }

		public bool isInitialized { get; private set; }
		
		protected ChessManager chessManager;
		protected ChessBoard chessBoard;
		protected ChessPiece chessPiece;
		
		public virtual void Initialize(ChessManager chessManager, ChessBoard chessBoard, ChessPiece chessPiece)
		{
			this.chessManager = chessManager;
			this.chessBoard = chessBoard;
			this.chessPiece = chessPiece;

			triggerables = new List<ITriggerableAction<ChessActionData>>();
			isInitialized = true;
		}
		
		public virtual void AddAction(ITriggerableAction<ChessActionData> triggerable)
		{
			if (!triggerables.Contains(triggerable))
			{
				triggerables.Add(triggerable);
				OnTriggered += triggerable.Trigger;
			}
		}

		public virtual void RemoveAction(ITriggerableAction<ChessActionData> triggerable)
		{
			if (triggerables.Contains(triggerable))
			{
				OnTriggered -= triggerable.Trigger;
				triggerables.Remove(triggerable);
			}
		}
		
		public virtual void TriggerActions(ChessActionData triggerData)
		{
			OnTriggered?.Invoke(triggerData);
		}

		public virtual void CleanUp()
		{
			if (!isInitialized)
				return;
			
			foreach (ITriggerableAction<ChessActionData> triggerableAction in triggerables)
				RemoveAction(triggerableAction);
		}
	}
}