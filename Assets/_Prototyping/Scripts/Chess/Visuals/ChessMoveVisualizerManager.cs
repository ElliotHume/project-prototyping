using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Visuals
{
	public class ChessMoveVisualizerManager : MonoBehaviour
	{
		[SerializeField]
		private ChessManager _chessManager;

		[SerializeField]
		private ChessMoveVisual _chessMoveVisualPrefab;

		private Dictionary<ChessPiece, ChessMoveVisual> _chessMoveVisuals;
		
		private void Start()
		{
			_chessMoveVisuals = new Dictionary<ChessPiece, ChessMoveVisual>();
			_chessManager.OnPlayerPieceMoveTasksChanged += OnPlayerPieceMoveTasksChanged;
		}

		private void OnDestroy()
		{
			_chessManager.OnPlayerPieceMoveTasksChanged -= OnPlayerPieceMoveTasksChanged;
		}

		private void OnPlayerPieceMoveTasksChanged(ChessPiece piece)
		{
			VisualizePlannedMoveTask(piece);
		}

		public void RefreshAllVisualizations()
		{
			foreach (ChessPiece piece in _chessManager.playerChessPieces)
			{
				VisualizePlannedMoveTask(piece);
			}
		}

		private void VisualizePlannedMoveTask(ChessPiece piece)
		{
			if (piece == null)
				return;
			
			bool isPlanned = _chessManager.playerMovePieceTasks.ContainsKey(piece);
			
			// If the player has cancelled the planned task, remove the visuals for it
			if (!isPlanned)
			{
				if (_chessMoveVisuals.ContainsKey(piece))
				{
					_chessMoveVisuals[piece]?.Cleanup();
					_chessMoveVisuals.Remove(piece);
				}
			} else
			{
				if (!_chessMoveVisuals.ContainsKey(piece))
				{
					ChessMoveVisual newVisual = Instantiate(_chessMoveVisualPrefab, transform);
					_chessMoveVisuals.Add(piece, newVisual);
				}
				
				_chessMoveVisuals[piece].Setup(piece, _chessManager.playerMovePieceTasks[piece]);
			}
			
		}
	}
}