using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess
{
	public class ChessBoardMovementHighlighter : MonoBehaviour
	{
		[SerializeField]
		private ChessBoard _chessBoard;

		private List<ChessBoardCell> _highlightedCells;

		private void Start()
		{
			_highlightedCells = new List<ChessBoardCell>();
		}
		
		public void HighlightMovementForPiece(ChessPiece newPiece)
		{
			foreach (ChessBoardCell chessBoardCell in _highlightedCells.ToArray())
			{
				chessBoardCell.ToggleHighlight(false);
				_highlightedCells.Remove(chessBoardCell);
			}
			
			if (newPiece != null)
			{
				List<Vector2Int> possibleMovementCoordinates = newPiece.GetPossibleMovementOptionCoordinates();
				List<ChessBoardCell> possibleMovementCells = _chessBoard.ConvertCoordinateListToBoardCells(possibleMovementCoordinates);
				foreach (ChessBoardCell cell in possibleMovementCells)
				{
					cell.ToggleHighlight(true);
					if (!_highlightedCells.Contains(cell))
						_highlightedCells.Add(cell);
				}
			}
		}
	}
}