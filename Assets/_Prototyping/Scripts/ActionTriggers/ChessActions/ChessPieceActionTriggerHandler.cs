using System.Collections.Generic;
using _Prototyping.ActionTriggers.ChessActions.TriggerableActions;
using _Prototyping.ActionTriggers.ChessActions.Triggers;
using _Prototyping.Chess;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Prototyping.ActionTriggers.ChessActions
{
	public class ChessPieceActionTriggerHandler : MonoBehaviour
	{
		[SerializeField]
		private ChessPiece _chessPiece;
		
		[SerializeField]
		private SerializedDictionary<ChessActionTrigger, ChessTriggerableAction> _initialSetup;

		private List<ChessActionTrigger> _actionTriggers;
		
		private ChessManager _chessManager;
		private ChessBoard _chessBoard;
		private bool _isInitialized;

		private void Start()
		{
			if (_chessPiece == null)
				_chessPiece = GetComponentInParent<ChessPiece>();
		}

		public void Initialize(ChessManager chessManager, ChessBoard board)
		{
			_chessManager = chessManager;
			_chessBoard = board;
			
			_actionTriggers = new List<ChessActionTrigger>();
			foreach (KeyValuePair<ChessActionTrigger, ChessTriggerableAction> kvp in _initialSetup)
			{
				if (!_actionTriggers.Contains(kvp.Key))
				{
					kvp.Key.Initialize(_chessManager, _chessBoard, _chessPiece);
					_actionTriggers.Add(kvp.Key);
				}

				kvp.Key.AddAction(kvp.Value);
			}

			_isInitialized = true;
		}

		public void AddTrigger(ChessActionTrigger newTrigger)
		{
			if (!_isInitialized)
			{
				Debug.LogError($"[{nameof(ChessPieceActionTriggerHandler)}] Tried to add a trigger to a handler that was not initialized.");
				return;
			}
			
			if (!_actionTriggers.Contains(newTrigger))
			{
				newTrigger.Initialize(_chessManager, _chessBoard, _chessPiece);
				_actionTriggers.Add(newTrigger);
			}
		}

		public void RemoveTrigger(ChessActionTrigger trigger, bool forceRemove = false)
		{
			if (_actionTriggers.Contains(trigger))
			{
				if (trigger.triggerables.Count != 0 && !forceRemove)
				{
					Debug.LogError($"[{nameof(ChessPieceActionTriggerHandler)}] You are trying to remove a trigger that still has triggerables listening to it, the removal has been cancelled.");
					return;
				}

				trigger.CleanUp();
				_actionTriggers.Remove(trigger);
			}
		}

		public void Cleanup()
		{
			foreach (ChessActionTrigger chessActionTrigger in _actionTriggers)
			{
				RemoveTrigger(chessActionTrigger, true);
			}
		}
	}
}