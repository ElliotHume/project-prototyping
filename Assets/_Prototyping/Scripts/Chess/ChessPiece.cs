using System.Collections.Generic;
using _Prototyping.Chess.Core;
using _Prototyping.Chess.Movement;
using _Prototyping.Grids.Core;
using _Prototyping.Grids;
using UnityEngine;

namespace _Prototyping.Chess
{
	public class ChessPiece : MonoBehaviour, IHasGridPosition<ChessBoardCell>, IChessPiece
	{
		[SerializeField]
		private ChessPieceConfig _config;
		
		[field: SerializeField]
		public bool isPlayerControlled { get; private set; }
		
		public ChessPieceType type => _config.type;
		public List<ChessMovementType> movementOptions { get; set; }
		
		public ChessBoardCell cell { get; private set; }
		public IGridCell<ChessBoardCell> Cell => cell;

		public IGrid<ChessBoardCell> grid => cell.grid;
		public Vector2Int gridCoordinates => cell.gridCoordinates;
		public int x => gridCoordinates.x;
		public int y => gridCoordinates.y;

		private void Start()
		{
			movementOptions = new List<ChessMovementType>(_config.movementOptions);
		}
	}
}