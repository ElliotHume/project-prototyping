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
		
		[field: SerializeField]
		public ChessBoardMovementHighlighter chessBoardMovementHighlighter { get; private set; }

		[SerializeField]
		private SelectablePointerHandler _selectablePointerHandler;

		[SerializeField]
		private ChessBoardSetupConfig _gameStartBoardSetup;

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

		private ChessPiece _selectedChessPiece;

		public ChessPiece selectedChessPiece
		{
			get => _selectedChessPiece;
			private set
			{
				_selectedChessPiece = value;
				OnSelectedPieceChanged?.Invoke(value);
			}
		}

		public Action<ChessPiece> OnSelectedPieceChanged;
		
		private void Awake()
		{
			Instance = this;
			chessPieces = new List<ChessPiece>();
		}

		private void Start()
		{
			gameState = ChessGameState.PlayerTurn;
			
			_selectablePointerHandler.OnPointerSelectionStarted += OnPointerSelectionStarted;
			OnSelectedPieceChanged += OnSelectedPieceChangedMethod;

			if (chessBoard.isInitialized)
			{
				SetupGameStartBoard();
			}
			else
			{
				chessBoard.OnInitialized += SetupGameStartBoard;
			}
		}

		private void OnDestroy()
		{
			_selectablePointerHandler.OnPointerSelectionStarted -= OnPointerSelectionStarted;
			chessBoard.OnInitialized -= SetupGameStartBoard;
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

		private void SetupGameStartBoard()
		{
			Debug.Log($"[{nameof(ChessManager)}] Starting Initial Board Setup...");
			foreach (KeyValuePair<Vector2Int, ChessPiece> kvpChessPiece in _gameStartBoardSetup.chessPieceMap)
			{
				ChessPiece newPiece = Instantiate(kvpChessPiece.Value, Vector3.up * 1000, Quaternion.identity);
				newPiece.MoveToCell(chessBoard.cells[kvpChessPiece.Key]);
			}
			
			Debug.Log($"[{nameof(ChessManager)}] Finished Initial Board Setup");
		}

		private void OnPointerSelectionStarted(IPointerSelectable selectable)
		{
			if (!canPlayerAct)
				return;
			
			if (selectable is ChessPieceSelectable chessPieceSelectable)
			{
				if (chessPieceSelectable.chessPiece.isPlayerControlled)
				{
					// TODO: Castling logic
					
					// If the selected new piece is player controlled, switch control to that piece
					if (selectedChessPiece != null)
						selectedChessPiece.chessPieceSelectable.ToggleSelection(false);
					selectedChessPiece = chessPieceSelectable.chessPiece;
					return;
				}
				
				if (selectedChessPiece != null && selectedChessPiece.chessPieceSelectable != chessPieceSelectable)
				{
					if (chessBoard.CanPieceTakeOther(selectedChessPiece, chessPieceSelectable.chessPiece))
					{
						PlayerMovePieceToCell(selectedChessPiece, chessPieceSelectable.chessPiece.cell);
						return;
					}
				}
			}
			else
			{
				if (selectedChessPiece != null)
					selectedChessPiece.chessPieceSelectable.ToggleSelection(false);
				selectedChessPiece = null;
			}
		}

		private void OnSelectedPieceChangedMethod(ChessPiece piece)
		{
			if (chessBoardMovementHighlighter != null)
				chessBoardMovementHighlighter.HighlightMovementForPiece(piece);
			
			if (piece != null)
				piece.chessPieceSelectable.ToggleSelection(true);
		}

		private void PlayerMovePieceToCell(ChessPiece piece, ChessBoardCell cell)
		{
			ChangeToResolvePlayerTurn();
			if (!cell.isEmpty && !cell.chessPiece.isPlayerControlled)
			{
				OnEnemyPieceTakenUnityEvent?.Invoke(cell.chessPiece);
				cell.chessPiece.Kill();
			}

			piece.MoveToCell(cell);
			ChangeToEnemyTurn();
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

			selectedChessPiece = null;
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