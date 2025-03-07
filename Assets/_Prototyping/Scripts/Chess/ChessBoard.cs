using System;
using System.Collections.Generic;
using System.Linq;
using _Prototyping.Grids.Core;
using UnityEngine;

namespace _Prototyping.Chess
{
	public class ChessBoard : MonoBehaviour, IGrid<ChessBoardCell>
	{
		[field: SerializeField]
		public Vector2Int initialDimensions { get; private set; }

		[SerializeField]
		private ChessBoardCell _cellPrefab;

		[SerializeField]
		private float _spaceBetweenCells = 1f;

		public Dictionary<Vector2Int, ChessBoardCell> cells { get; private set; }

		public bool isInitialized { get; private set; }
		public Action OnInitialized;
		
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

		private void Start()
		{
			InitiateCells();
		}

		private void InitiateCells()
		{
			// Destroy previous cells, if they exist
			if (cells != null && cells.Count > 0)
				foreach (ChessBoardCell cell in cells.Values)
					Destroy(cell);
			
			cells = new Dictionary<Vector2Int, ChessBoardCell>();
			
			Vector3 startPosition = transform.position;
			Quaternion rotation = transform.rotation;
			for (int i = 0; i < initialDimensions.x; i++)
			{
				for (int j = 0; j < initialDimensions.y; j++)
				{
					Vector2Int coordinates = new Vector2Int(i, j);
					ChessBoardCell spawnedCell = Instantiate(_cellPrefab,
						startPosition + new Vector3(_spaceBetweenCells * i, 0, _spaceBetweenCells * j), rotation,
						transform);
					spawnedCell.Instantiate(this, coordinates);
					Debug.Log("SPAWNED CELL");
					cells.Add(coordinates, spawnedCell);
				}
			}
			
			Debug.Log($"[{nameof(ChessBoard)}] Initialized Board");

			isInitialized = true;
			OnInitialized?.Invoke();
		}

		public bool IsPositionOnBoard(Vector2Int position)
		{
			return cells.ContainsKey(position);
		}

		public bool IsPositionAvailable(Vector2Int position)
		{
			if (!IsPositionOnBoard(position))
				return false;

			if (!cells[position].isEmpty)
				return false;

			return true;
		}

		public List<ChessBoardCell> ConvertCoordinateListToBoardCells(List<Vector2Int> coordinateList)
		{
			List<ChessBoardCell> cellsList = new List<ChessBoardCell>();
			foreach (Vector2Int coordinate in coordinateList)
			{
				cellsList.Add(cells[coordinate]);
			}
			return cellsList;
		}
		
		public bool ArePiecesOnDifferentTeams(ChessPiece firstPiece, ChessPiece secondPiece)
		{
			return firstPiece.isPlayerControlled != secondPiece.isPlayerControlled;
		}

		public bool CanPieceMoveToCell(ChessPiece selectedPiece, ChessBoardCell targetCell)
		{
			Debug.Log($"[{nameof(ChessBoard)}] Check can piece [{selectedPiece}] move to cell [{targetCell.gridCoordinates}]");
			List<Vector2Int> possibleCoordinateOptions = selectedPiece.GetPossibleMovementOptionCoordinates();
			bool movePossible = possibleCoordinateOptions.Contains(targetCell.gridCoordinates);
			
			// TODO: Check for castling
			bool noBlockingPiece = targetCell.isEmpty
									|| (!targetCell.chessPiece.isPlayerControlled && ArePiecesOnDifferentTeams(selectedPiece, targetCell.chessPiece));
			
			return movePossible && noBlockingPiece;
		}

		public bool CanPieceTakeOther(ChessPiece selectedPiece, ChessPiece targetPiece)
		{
			return CanPieceMoveToCell(selectedPiece, targetPiece.cell);
		}

		public List<ChessPiece> GetListOfTakeablePieces(ChessPiece selectedPiece)
		{
			List<Vector2Int> possibleCoordinateOptions = selectedPiece.GetPossibleMovementOptionCoordinates()
				.Where((coord) => !cells[coord].isEmpty && ArePiecesOnDifferentTeams(selectedPiece, cells[coord].chessPiece)).ToList();
			return possibleCoordinateOptions.Select((coord) => cells[coord].chessPiece).ToList();
		}

		// TODO: replace boolean with an enum for what kind of attack it was (piece take, AoE attack, etc...)
		public void KillPieceOnCell(ChessBoardCell targetCell, bool isPieceTake = true)
		{
			if (!targetCell.isEmpty)
			{
				Debug.Log($"[{nameof(ChessManager)}] Kill piece {targetCell.chessPiece}");
				targetCell.chessPiece.Kill(targetCell.chessPiece);	
			}
		}
		

		public void PrintBoardState()
		{
			string stateString = "-------------------Board State------------------------";
			foreach (KeyValuePair<Vector2Int, ChessBoardCell> kvp in cells)
			{
				stateString += $"\n[{kvp.Key}] - piece: {(kvp.Value.isEmpty ? "Empty" : kvp.Value.chessPiece)}";
			}

			Debug.Log(stateString);
		}
	}
}