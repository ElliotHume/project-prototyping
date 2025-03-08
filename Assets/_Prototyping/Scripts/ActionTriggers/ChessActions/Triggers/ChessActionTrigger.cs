using System;
using System.Collections.Generic;
using _Prototyping.ActionTriggers.ChessActions.Interfaces;
using _Prototyping.ActionTriggers.Core;
using _Prototyping.Chess;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Prototyping.ActionTriggers.ChessActions.Triggers
{
	public abstract class ChessActionTrigger : ScriptableObject, IChessActionTrigger<ChessActionTrigger>
	{
		public abstract string triggerId { get; }
		
		public List<ITriggerableAction<ChessActionData>> triggerables { get; private set; }
		public Action<ChessActionData> OnTriggered { get; set; }

		public bool isInitialized { get; private set; }
		
		protected ChessManager chessManager;
		protected ChessBoard chessBoard;
		protected ChessPiece chessPiece;
		
		public virtual ChessActionTrigger InitializeInstance(ChessManager chessManager, ChessBoard chessBoard, ChessPiece chessPiece)
		{
			ChessActionTrigger instance = Object.Instantiate(this);
			instance.chessManager = chessManager;
			instance.chessBoard = chessBoard;
			instance.chessPiece = chessPiece;

			instance.triggerables = new List<ITriggerableAction<ChessActionData>>();
			instance.isInitialized = true;

			return instance;
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