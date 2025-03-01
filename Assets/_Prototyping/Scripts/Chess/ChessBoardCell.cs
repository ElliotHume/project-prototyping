using _Prototyping.Grids.Core;
using UnityEngine;

namespace _Prototyping.Chess
{
	public class ChessBoardCell : MonoBehaviour, IGridCell<ChessBoardCell>
	{
		public IGrid<ChessBoardCell> grid { get; private set; }
		public Vector2Int gridCoordinates { get; private set; }
		public int x => gridCoordinates.x;
		public int y => gridCoordinates.y;

		public ChessPiece chessPiece;
		public bool isEmpty => chessPiece != null;
		
		private ChessTileVisuals _chessTileVisuals;

		private void Awake()
		{
			_chessTileVisuals = GetComponentInChildren<ChessTileVisuals>();
		}

		public void Instantiate(ChessBoard chess, Vector2Int coordinates)
		{
			this.grid = chess;
			this.gridCoordinates = coordinates;
			
			_chessTileVisuals.UpdateVisuals(this);
		}
	}
}