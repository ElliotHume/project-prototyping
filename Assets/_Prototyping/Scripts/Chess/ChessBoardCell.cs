using _Prototyping.Grids.Core;
using UnityEngine;
using UnityEngine.Events;

namespace _Prototyping.Chess
{
	public class ChessBoardCell : MonoBehaviour, IGridCell<ChessBoardCell>
	{
		[field: SerializeField]
		public Transform piecePositionTransform { get; private set; }
		
		public ChessBoard board { get; private set; }
		public IGrid<ChessBoardCell> grid => board;
		public Vector2Int gridCoordinates { get; private set; }
		public int x => gridCoordinates.x;
		public int y => gridCoordinates.y;

		public ChessPiece chessPiece { get; private set; }
		public bool isEmpty => chessPiece != null;

		public UnityEvent<ChessPiece> OnPieceMovedToCell;
		
		private ChessTileVisuals _chessTileVisuals;

		private void Awake()
		{
			_chessTileVisuals = GetComponentInChildren<ChessTileVisuals>();
		}

		public void Instantiate(ChessBoard board, Vector2Int coordinates)
		{
			this.board = board;
			this.gridCoordinates = coordinates;
			
			_chessTileVisuals.UpdateVisuals(this);
		}

		public void ToggleHighlight(bool toggle)
		{
			_chessTileVisuals.ToggleHighlight(toggle);
		}

		public void SetPiece(ChessPiece piece)
		{
			chessPiece = piece;
			OnPieceMovedToCell?.Invoke(chessPiece);
		}
	}
}