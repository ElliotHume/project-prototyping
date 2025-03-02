using System.Collections.Generic;
using _Prototyping.Chess.Core;
using _Prototyping.Chess.Movement;
using _Prototyping.Grids.Core;
using _Prototyping.PointerSelectables;
using _Prototyping.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace _Prototyping.Chess
{
	public class ChessPiece : MonoBehaviour, IHasGridPosition<ChessBoardCell>, IChessPiece
	{
		[SerializeField]
		private ChessPieceConfig _config;
		
		[field: SerializeField]
		public bool isPlayerControlled { get; private set; }
		
		[field: SerializeField]
		public ChessPieceSelectable chessPieceSelectable { get; private set; }

		[SerializeField]
		private Transform _tileSnapPointTransform;

		[SerializeField]
		private GameObject _deathVFXPrefab;
		
		public ChessPieceType type => _config.type;
		public List<ChessMovementType> movementOptions { get; set; }
		
		public IGridCell<ChessBoardCell> Cell => cell;

		public IGrid<ChessBoardCell> grid => cell.grid;
		public Vector2Int gridCoordinates => cell.gridCoordinates;
		public int x => gridCoordinates.x;
		public int y => gridCoordinates.y;

		public UnityEvent<ChessBoardCell> OnChangedCells;
		
		[field: Header("DEBUG")]
		[field: SerializeField]
		public ChessBoardCell cell { get; private set; }
		
		private void Start()
		{
			movementOptions = new List<ChessMovementType>(_config.movementOptions);
			if (chessPieceSelectable == null)
				chessPieceSelectable = GetComponentInChildren<ChessPieceSelectable>();
			
			ChessManager.Instance.RegisterChessPiece(this);
		}

		public void MoveToCell(ChessBoardCell newCell)
		{
			cell = newCell;
			cell.SetPiece(this);
			
			// Position transform of piece onto the tile
			transform.position = cell.piecePositionTransform.position;
			transform.rotation = cell.piecePositionTransform.rotation;
			
			OnChangedCells?.Invoke(newCell);
		}

		public List<Vector2Int> GetPossibleMovementOptionCoordinates()
		{
			if (cell == null)
				return null;
			
			List<Vector2Int> coordinateOptions = new List<Vector2Int>();
			foreach (ChessMovementType movementType in movementOptions)
			{
				coordinateOptions.AddRange(movementType.GetPossibleMovePositions(this, cell.board));
			}
			return coordinateOptions;
		}

		public void Kill()
		{
			if (_deathVFXPrefab != null)
				Instantiate(_deathVFXPrefab, transform.position, transform.rotation);
			ChessManager.Instance.UnregisterChessPiece(this);
			Destroy(this);
		}
	}
}