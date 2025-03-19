using System;
using System.Collections.Generic;
using System.Linq;
using _Prototyping.Chess.Scheduler;
using _Prototyping.Chess.Scheduler.Tasks;
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
		private ChessScheduler _scheduler;
		public bool canPlayerAct => gameState == ChessGameState.PlayerTurn && _scheduler.CheckCanMoveToNextGameState(gameState);
		public bool canEnemyAct => gameState == ChessGameState.EnemyTurn && _scheduler.CheckCanMoveToNextGameState(gameState);

		public Dictionary<ChessPiece, PlayerMovePieceTask> playerMovePieceTasks;
		public Dictionary<ChessPiece, EnemyMovePieceTask> enemyMovePieceTasks;

		public Action<ChessPiece> OnPlayerPieceMoveTasksChanged;
		public Action<ChessPiece> OnEnemyPieceMoveTasksChanged;

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
			playerMovePieceTasks = new Dictionary<ChessPiece, PlayerMovePieceTask>();
			enemyMovePieceTasks = new Dictionary<ChessPiece, EnemyMovePieceTask>();
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
		
		private void OnDestroy()
		{
			chessBoard.OnInitialized -= SetupGameStartBoard;
		}

		public void RegisterChessPiece(ChessPiece chessPiece)
		{
			if (!chessPieces.Contains(chessPiece))
			{
				chessPiece.chessPieceActionTriggerHandler.Initialize(this, chessBoard);
				chessPieces.Add(chessPiece);
				OnRegisteredChessPiece?.Invoke(chessPiece);
			}
		}
		
		public void UnregisterChessPiece(ChessPiece chessPiece)
		{
			if (chessPieces.Contains(chessPiece))
			{
				chessPiece.chessPieceActionTriggerHandler.Cleanup();
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

		private void FixedUpdate()
		{
			if (_scheduler == null)
				_scheduler = ChessScheduler.Instance;

			switch (gameState)
			{
				case ChessGameState.Paused:
					break;
				case ChessGameState.PlayerTurn:
					ProcessPlayerTurn();
					break;
				case ChessGameState.ResolvingPlayerTurn:
					ProcessResolvePlayerTurn();
					break;
				case ChessGameState.EnemyTurn:
					ProcessEnemyTurn();
					break;
				case ChessGameState.ResolvingEnemyTurn:
					ProcessResolveEnemyTurn();
					break;
			}
			
			_scheduler.UpdateTasks(gameState);
		}
		
		private void ChangeToPlayerTurn()
		{
			gameState = ChessGameState.PlayerTurn;
			OnStartPlayerTurnUnityEvent?.Invoke();
		}
		
		private void ChangeToResolvePlayerTurn()
		{
			selectedChessPiece = null;
			gameState = ChessGameState.ResolvingPlayerTurn;
			OnResolvePlayerTurnUnityEvent?.Invoke();
		}
		
		private void ChangeToResolveEnemyTurn()
		{
			gameState = ChessGameState.ResolvingEnemyTurn;
			OnResolveEnemyTurnUnityEvent?.Invoke();
		}
		
		private void ChangeToEnemyTurn()
		{
			gameState = ChessGameState.EnemyTurn;
			OnStartEnemyTurnUnityEvent?.Invoke();
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

			Debug.Log($"[{selectable == _cachedSelectable}]  [{previousSelectable == _cachedPreviousSelectable}]");

			if (selectable is ChessPieceSelectable chessPieceSelectable)
			{
				// If the selectable piece is player controlled, switch the selected piece to it
				if (chessPieceSelectable.chessPiece.isPlayerControlled)
				{
					// TODO: Castling logic
					
					// If the selected new piece is player controlled, switch control to that piece
					selectedChessPiece = chessPieceSelectable.chessPiece;
				}
				// If the selectable is not player controlled, check if we previously had selected a player piece
				else if (previousSelectable is ChessPieceSelectable previousChessPieceSelectable)
				{
					if (previousChessPieceSelectable.chessPiece.isPlayerControlled)
					{
						if (chessBoard.CanPieceTakeOther(previousChessPieceSelectable.chessPiece, chessPieceSelectable.chessPiece))
						{
							PlayerMovePieceToCell(previousChessPieceSelectable.chessPiece, chessPieceSelectable.chessPiece.cell);
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
						}
					}
				}
			}
			
			_cachedSelectable = selectable;
			_cachedPreviousSelectable = previousSelectable;
		}

		private void ProcessResolvePlayerTurn()
		{
			if (_scheduler.CheckCanMoveToNextGameState(ChessGameState.ResolvingPlayerTurn))
			{
				ChangeToEnemyTurn();
			}
		}
		
		private void ProcessEnemyTurn()
		{
			foreach (ChessPiece enemyPiece in enemyChessPieces)
			{
				(ChessPiece targetPiece, int influenceDifference)? bestTake = GetBestPossiblePieceTake(enemyPiece);
				if (bestTake.HasValue)
				{
					EnemyMovePieceToCell(enemyPiece, bestTake.Value.targetPiece.cell);
				}
				else
				{
					ChessBoardCell randomMove = GetRandomMove(enemyPiece);
					EnemyMovePieceToCell(enemyPiece, randomMove);
				}
			}
			EndEnemyTurn();
		}

		private void ProcessResolveEnemyTurn()
		{
			if (_scheduler.CheckCanMoveToNextGameState(ChessGameState.ResolvingEnemyTurn))
			{
				ChangeToPlayerTurn();
			}
		}
		
		private void OnSelectedPieceChangedMethod(ChessPiece piece)
		{
			if (chessBoardMovementHighlighter != null)
				chessBoardMovementHighlighter.HighlightMovementForPiece(piece);
		}

		private void PlayerMovePieceToCell(ChessPiece piece, ChessBoardCell cell)
		{
			AddPlayerMoveTask(piece, new PlayerMovePieceTask(this, chessBoard, piece, cell));
		}

		private void AddPlayerMoveTask(ChessPiece piece, PlayerMovePieceTask task, bool cancelPrevious = true)
		{
			if (cancelPrevious && playerMovePieceTasks.ContainsKey(piece))
				CancelPlayerMoveTask(piece);
			
			_scheduler.AddScheduledTask(ChessGameState.ResolvingPlayerTurn, task);
			playerMovePieceTasks.Add(piece, task);
			OnPlayerPieceMoveTasksChanged?.Invoke(piece);
		}

		private void CancelPlayerMoveTask(ChessPiece piece)
		{
			if (!playerMovePieceTasks.ContainsKey(piece))
				return;
			
			_scheduler.RemoveTask(playerMovePieceTasks[piece], ChessGameState.ResolvingPlayerTurn);
			playerMovePieceTasks.Remove(piece);
			OnPlayerPieceMoveTasksChanged?.Invoke(piece);
		}

		public void EndPlayerTurn()
		{ 
			ChangeToResolvePlayerTurn();
		}
		
		private (ChessPiece selectedPiece, ChessPiece targetPiece, int influenceDifference) GetBestPossiblePieceTakeForSet(List<ChessPiece> piecesToCheck)
		{
			int bestInfluenceDiff = -999;
			ChessPiece chosenPieceToMove = null;
			ChessPiece targetPieceToTake = null;
			foreach (ChessPiece piece in piecesToCheck)
			{
				(ChessPiece targetPiece, int influenceDifference)? bestPieceTake = GetBestPossiblePieceTake(piece);
				if (bestPieceTake.HasValue)
				{
					if (bestPieceTake.Value.influenceDifference > bestInfluenceDiff)
					{
						bestInfluenceDiff = bestPieceTake.Value.influenceDifference;
						chosenPieceToMove = piece;
						targetPieceToTake = bestPieceTake.Value.targetPiece;
					}
				}
			}

			return (chosenPieceToMove, targetPieceToTake, bestInfluenceDiff);
		}

		private (ChessPiece targetPiece, int influenceDifference)? GetBestPossiblePieceTake(
			ChessPiece pieceToCheck)
		{
			ChessPiece highestInfluenceTakeablePiece = FindHighestInfluencePiece(chessBoard.GetListOfTakeablePieces(pieceToCheck));
			if (highestInfluenceTakeablePiece != null)
				return (highestInfluenceTakeablePiece, highestInfluenceTakeablePiece.influence - pieceToCheck.influence);
			
			return null;
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

			// TODO: Fill this out with an algorithm that goes through each piece and tries to find a move that creates the best piece take next turn

			return (chosenPieceToMove, targetCell);
		}

		private (ChessPiece selectedPiece, ChessBoardCell targetCell) GetRandomMoveInSet(List<ChessPiece> piecesToCheck)
		{
			if (piecesToCheck.Count == 0)
				return (null, null);
			
			int randomIndex = Random.Range(0, piecesToCheck.Count);
			
			ChessPiece selectedPiece = piecesToCheck[randomIndex];

			return (selectedPiece, GetRandomMove(selectedPiece));
		}

		private ChessBoardCell GetRandomMove(ChessPiece piece)
		{
			List<Vector2Int> possibleMovementOptions = piece.GetPossibleMovementOptionCoordinates();
			int randomIndex = Random.Range(0, possibleMovementOptions.Count);
			ChessBoardCell targetCell = chessBoard.cells[possibleMovementOptions[randomIndex]];
			return targetCell;
		}
		
		
		private void EnemyMovePieceToCell(ChessPiece piece, ChessBoardCell cell)
		{
			_scheduler.AddScheduledTask(ChessGameState.ResolvingEnemyTurn, new EnemyMovePieceTask(this, chessBoard, piece, cell));
		}

		private void EndEnemyTurn()
		{
			ChangeToResolveEnemyTurn();
		}
	}
}