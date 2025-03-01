using System;
using System.Collections.Generic;
using System.Linq;
using _Prototyping.PointerSelectables;
using _Prototyping.PointerSelectables.Core;
using UnityEngine;
using UnityEngine.Events;

namespace _Prototyping.Chess
{
	public class ChessManager : MonoBehaviour
	{
		public static ChessManager Instance;
		public enum ChessGameState
		{
			Paused = 0, // Unused for now
			PlayerTurn = 1,
			ResolvingPlayerTurn = 2,
			EnemyTurn = 3,
			ResolvingEnemyTurn = 4,
		}

		[field: SerializeField]
		public ChessBoard chessBoard { get; private set; }

		[SerializeField]
		private SelectablePointerHandler _selectablePointerHandler;

		public List<ChessPiece> chessPieces { get; private set; }
		public List<ChessPiece> playerChessPieces => chessPieces.Where((piece) => piece.isPlayerControlled).ToList();
		public List<ChessPiece> enemyChessPieces => chessPieces.Where((piece) => !piece.isPlayerControlled).ToList();
		public Action<ChessPiece> OnRegisteredChessPiece;
		public Action<ChessPiece> OnUnregisteredChessPiece;
		
		public ChessGameState gameState;
		private ChessGameState _cachedGameState;
		public bool canPlayerAct => gameState == ChessGameState.PlayerTurn;
		public bool canEnemyAct => gameState == ChessGameState.EnemyTurn;

		public UnityEvent onPauseGameUnityEvent;
		public UnityEvent OnStartPlayerTurnUnityEvent;
		public UnityEvent OnResolvePlayerTurnUnityEvent;
		public UnityEvent OnStartEnemyTurnUnityEvent;
		public UnityEvent OnResolveEnemyTurnUnityEvent;

		public UnityEvent<ChessPiece> OnPlayerPieceTakenUnityEvent;
		public UnityEvent<ChessPiece> OnEnemyPieceTakenUnityEvent;
		
		public ChessPiece selectedChessPiece { get; private set; }
		
		private void Awake()
		{
			Instance = this;
		}

		private void Start()
		{
			gameState = ChessGameState.PlayerTurn;
			
			_selectablePointerHandler.OnPointerSelectionStarted += OnPointerSelectionStarted;
		}

		public void RegisterChessPiece(ChessPiece chessPiece)
		{
			if (!chessPieces.Contains(chessPiece))
			{
				chessPieces.Add(chessPiece);
				OnRegisteredChessPiece?.Invoke(chessPiece);
			}
				
		}
		
		public void UnregisterChessPiece(ChessPiece chessPiece)
		{
			if (chessPieces.Contains(chessPiece))
			{
				chessPieces.Remove(chessPiece);
				OnUnregisteredChessPiece?.Invoke(chessPiece);
			}
		}

		private void OnPointerSelectionStarted(IPointerSelectable selectable)
		{
			// Player's cannot act when it is not their turn.
			if (gameState != ChessGameState.PlayerTurn)
				return;
			
			if (selectable is ChessPieceSelectable chessPieceSelectable)
			{
				if (selectedChessPiece != null && selectedChessPiece.chessPieceSelectable != chessPieceSelectable)
				{
					// TODO: Castling logic
					
					// If the selected new piece is player controlled, switch control to that piece
					if (chessPieceSelectable.chessPiece.isPlayerControlled)
					{
						selectedChessPiece = chessPieceSelectable.chessPiece;
						return;
					}
				}
			}
		}

		private void PlayerMovePieceToCell(ChessPiece piece, ChessBoardCell cell)
		{
			if (!cell.isEmpty)
			{
						
			}
		}

		private void ChangeToPlayerTurn()
		{
			// If moving from an incorrect game state, do not proceed.
			if (gameState == ChessGameState.PlayerTurn || gameState == ChessGameState.EnemyTurn)
				return;

			gameState = ChessGameState.PlayerTurn;
			OnStartPlayerTurnUnityEvent?.Invoke();
		}
		
		private void ChangeToResolvePlayerTurn()
		{
			// If moving from an incorrect game state, do not proceed.
			if (gameState != ChessGameState.PlayerTurn)
				return;

			gameState = ChessGameState.ResolvingPlayerTurn;
			OnResolvePlayerTurnUnityEvent?.Invoke();
		}
		
		private void ChangeToEnemyTurn()
		{
			// If moving from an incorrect game state, do not proceed.
			if (gameState == ChessGameState.PlayerTurn || gameState == ChessGameState.EnemyTurn)
				return;

			gameState = ChessGameState.EnemyTurn;
			OnStartEnemyTurnUnityEvent?.Invoke();
		}
		
		private void ChangeToResolveEnemyTurn()
		{
			// If moving from an incorrect game state, do not proceed.
			if (gameState != ChessGameState.EnemyTurn)
				return;

			gameState = ChessGameState.ResolvingEnemyTurn;
			OnResolveEnemyTurnUnityEvent?.Invoke();
		}
	}
}