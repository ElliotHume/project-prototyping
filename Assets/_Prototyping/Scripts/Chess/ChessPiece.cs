using System;
using System.Collections.Generic;
using _Prototyping.ActionTriggers.ChessActions;
using _Prototyping.Chess.Core;
using _Prototyping.Chess.Movement;
using _Prototyping.Grids.Core;
using _Prototyping.PointerSelectables;
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
		
		[field: SerializeField]
		public ChessPieceActionTriggerHandler chessPieceActionTriggerHandler { get; private set; }

		[SerializeField]
		private Transform _tileSnapPointTransform;

		[SerializeField]
		private GameObject _deathVFXPrefab;
		
		public ChessPieceType type => _config.type;
		
		public int dynamicInfluence { get; private set; }
		public int influence => _config.baseInfluence + dynamicInfluence;
		public List<ChessMovementType> movementOptions { get; set; }
		
		public IGridCell<ChessBoardCell> Cell => cell;

		public IGrid<ChessBoardCell> grid => cell.grid;
		public Vector2Int gridCoordinates => cell.gridCoordinates;
		public int x => gridCoordinates.x;
		public int y => gridCoordinates.y;

		public UnityEvent<ChessBoardCell> OnChangedCellsUnityEvent;

		public Action<ChessPiece> OnThisPieceTaken;
		public Action<ChessPiece> OnPieceTakeOther;

		private ChessManager _chessManager;
		public ChessManager chessManager
		{
			get
			{
				if (_chessManager == null)
					_chessManager = ChessManager.Instance;
				return _chessManager;
			}
		}
		
		[field: Header("DEBUG")]
		[field: SerializeField]
		public ChessBoardCell cell { get; private set; }
		
		private void Start()
		{
			movementOptions = new List<ChessMovementType>(_config.movementOptions);
			if (chessPieceSelectable == null)
				chessPieceSelectable = GetComponentInChildren<ChessPieceSelectable>();
			if (chessPieceActionTriggerHandler == null)
				chessPieceActionTriggerHandler = GetComponentInChildren<ChessPieceActionTriggerHandler>();
			
			ChessManager.Instance.RegisterChessPiece(this);
		}

		public void MoveToCell(ChessBoardCell newCell)
		{
			if (cell != null)
				cell.SetPiece(null);
			
			cell = newCell;
			cell.SetPiece(this);
			
			// Position transform of piece onto the tile
			transform.position = cell.piecePositionTransform.position;
			transform.rotation = cell.piecePositionTransform.rotation;
			
			OnChangedCellsUnityEvent?.Invoke(newCell);
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

		public void Kill(ChessPiece killer)
		{
			OnThisPieceTaken?.Invoke(killer);
			killer.OnPieceTakeOther?.Invoke(this);
			
			if (_deathVFXPrefab != null)
				Instantiate(_deathVFXPrefab, transform.position, transform.rotation);
			
			cell.SetPiece(null);
			ChessManager.Instance.UnregisterChessPiece(this);
			Destroy(gameObject);
		}
	}
}