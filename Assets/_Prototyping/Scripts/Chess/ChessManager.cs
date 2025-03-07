using System;
using System.Collections.Generic;
using System.Linq;
using _Prototyping.PointerSelectables;
using _Prototyping.PointerSelectables.Core;
using UnityEngine;
using UnityEngine.Events;
using Random = UnityEngine.Random;

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
		private IPointerSelectable _cachedSelectable;
		private IPointerSelectable _cachedPreviousSelectable;

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

		private void FixedUpdate()
		{
			switch (gameState)
			{
				case ChessGameState.Paused:
					break;
				case ChessGameState.PlayerTurn:
					ProcessPlayerTurn();
					break;
				case ChessGameState.ResolvingPlayerTurn:
					break;
				case ChessGameState.EnemyTurn:
					ProcessEnemyTurn();
					break;
				case ChessGameState.ResolvingEnemyTurn:
					break;
			}
		}

		private void OnDestroy()
		{
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

		private void ProcessPlayerTurn()
		{
			if (!canPlayerAct)
				return;
			
			// Get selected player piece
			IPointerSelectable selectable = _selectablePointerHandler.selectedSelectable;
			IPointerSelectable previousSelectable = _selectablePointerHandler.previousSelectedSelectable;

			// If nothing selectable has changed, do not proceed past this point.
			if (selectable == _cachedSelectable && previousSelectable == _cachedPreviousSelectable)
				return;

			if (selectable == null)
			{
				selectedChessPiece = null;
				return;
			}
			
			if (selectable == previousSelectable)
				return;

			if (selectable is ChessPieceSelectable chessPieceSelectable)
			{
				// If the selectable piece is player controlled, switch the selected piece to it
				if (chessPieceSelectable.chessPiece.isPlayerControlled)
				{
					// TODO: Castling logic
					
					// If the selected new piece is player controlled, switch control to that piece
					selectedChessPiece = chessPieceSelectable.chessPiece;
					return;
				}
				
				// If the selectable is not player controlled, check if we previously had selected a player piece
				if (previousSelectable is ChessPieceSelectable previousChessPieceSelectable)
				{
					if (previousChessPieceSelectable.chessPiece.isPlayerControlled)
					{
						if (chessBoard.CanPieceTakeOther(previousChessPieceSelectable.chessPiece, chessPieceSelectable.chessPiece))
						{
							PlayerMovePieceToCell(previousChessPieceSelectable.chessPiece, chessPieceSelectable.chessPiece.cell);
							return;
						}
					}
				}
			} else if (selectable is ChessBoardCellSelectable chessBoardCellSelectable)
			{
				if (previousSelectable is ChessPieceSelectable previousChessPieceSelectable)
				{
					if (previousChessPieceSelectable.chessPiece.isPlayerControlled)
					{
						if (chessBoard.CanPieceMoveToCell(previousChessPieceSelectable.chessPiece, chessBoardCellSelectable.cell))
						{
							PlayerMovePieceToCell(previousChessPieceSelectable.chessPiece, chessBoardCellSelectable.cell);
							return;
						}
					}
				}
			}

			_cachedSelectable = selectable;
			_cachedPreviousSelectable = previousSelectable;
		}
		
		private void OnSelectedPieceChangedMethod(ChessPiece piece)
		{
			if (chessBoardMovementHighlighter != null)
				chessBoardMovementHighlighter.HighlightMovementForPiece(piece);
		}

		private void PlayerMovePieceToCell(ChessPiece piece, ChessBoardCell cell)
		{
			ChangeToResolvePlayerTurn();
			if (!cell.isEmpty && !cell.chessPiece.isPlayerControlled)
			{
				OnPlayerPieceTakenUnityEvent?.Invoke(cell.chessPiece);
				cell.chessPiece.Kill(piece);
			}

			piece.MoveToCell(cell);
			chessBoard.PrintBoardState();
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

		private void ProcessEnemyTurn()
		{
			(ChessPiece selectedPiece, ChessPiece targetPiece, int influenceDifference) bestPieceTake = GetBestPossiblePieceTake(enemyChessPieces);
			if (bestPieceTake.selectedPiece != null && bestPieceTake.targetPiece != null)
			{
				Debug.Log("Found attack move!");
				EnemyMovePieceToCell(bestPieceTake.selectedPiece, bestPieceTake.targetPiece.cell);
				return;
			}
			
			// TODO: Add support to find best possible setup move, before defaulting to a random move
			
			(ChessPiece selectedPiece, ChessBoardCell targetCell) randomMove = GetRandomMove(enemyChessPieces);
			Debug.Log("Moving random piece");
			EnemyMovePieceToCell(randomMove.selectedPiece, randomMove.targetCell);
			
		}

		private (ChessPiece selectedPiece, ChessPiece targetPiece, int influenceDifference) GetBestPossiblePieceTake(List<ChessPiece> piecesToCheck)
		{
			int bestInfluenceDiff = -999;
			ChessPiece chosenPieceToMove = null;
			ChessPiece targetPieceToTake = null;
			foreach (ChessPiece piece in piecesToCheck)
			{
				Debug.Log("Checking piece: "+piece);
				ChessPiece highestInfluenceTakeablePiece = FindHighestInfluencePiece(chessBoard.GetListOfTakeablePieces(piece));
				if (highestInfluenceTakeablePiece != null)
				{
					int influenceDiff = highestInfluenceTakeablePiece.influence - piece.influence;
					if (influenceDiff > bestInfluenceDiff)
					{
						bestInfluenceDiff = influenceDiff;
						chosenPieceToMove = piece;
						targetPieceToTake = highestInfluenceTakeablePiece;
					}
				}
			}

			return (chosenPieceToMove, targetPieceToTake, bestInfluenceDiff);
		} 

		private ChessPiece FindHighestInfluencePiece(List<ChessPiece> pieces)
		{
			if (pieces.Count == 0)
				return null;

			int highestInfluence = pieces[0].influence;
			ChessPiece highestInfluencePiece = pieces[0];
			for (int i = 1; i < pieces.Count; i++)
			{
				if (pieces[i].influence > highestInfluence
					|| (pieces[i].influence == highestInfluence && Random.value >= 0.5f))
				{
					highestInfluence = pieces[i].influence;
					highestInfluencePiece = pieces[i];
				}
			}

			return highestInfluencePiece;
		}

		/// <summary>
		/// Finds the best movement for a set of pieces that will set up a turn with a good piece-take
		/// </summary>
		private (ChessPiece selectedPiece, ChessBoardCell targetCell) GetBestPossibleSetupMove(
			List<ChessPiece> piecesToCheck)
		{
			ChessPiece chosenPieceToMove = null;
			ChessBoardCell targetCell = null;


			return (chosenPieceToMove, targetCell);
		}

		private (ChessPiece selectedPiece, ChessBoardCell targetCell) GetRandomMove(List<ChessPiece> piecesToCheck)
		{
			if (piecesToCheck.Count == 0)
				return (null, null);
			
			int randomIndex = Random.Range(0, piecesToCheck.Count);
			
			ChessPiece selectedPiece = piecesToCheck[randomIndex];
			List<Vector2Int> possibleMovementOptions = selectedPiece.GetPossibleMovementOptionCoordinates();
			
			randomIndex = Random.Range(0, possibleMovementOptions.Count);
			ChessBoardCell targetCell = chessBoard.cells[possibleMovementOptions[randomIndex]];

			return (selectedPiece, targetCell);
		}
		
		
		private void EnemyMovePieceToCell(ChessPiece piece, ChessBoardCell cell)
		{
			ChangeToResolveEnemyTurn();
			Debug.Log($"[{nameof(ChessManager)}] Move piece {piece} to cell {cell.gridCoordinates}");
			if (!cell.isEmpty && cell.chessPiece.isPlayerControlled)
			{
				OnEnemyPieceTakenUnityEvent?.Invoke(cell.chessPiece);
				Debug.Log($"[{nameof(ChessManager)}] Kill piece {cell.chessPiece}");
				cell.chessPiece.Kill(piece);
			}
			piece.MoveToCell(cell);
			chessBoard.PrintBoardState();
			ChangeToPlayerTurn();
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