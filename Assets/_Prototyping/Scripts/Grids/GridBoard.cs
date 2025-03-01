using System.Collections.Generic;
using _Prototyping.Grids.Core;
using UnityEngine;

namespace _Prototyping.Grids
{
	public class GridBoard : MonoBehaviour, IGrid<GridBoardCell>
	{
		[field: SerializeField]
		public Vector2Int dimensions { get; private set; }

		public int width => dimensions.x;
		public int height => dimensions.y;

		[SerializeField]
		private GridBoardCell _cellPrefab;

		[SerializeField]
		private float _spaceBetweenCells = 1f;

		public Dictionary<Vector2Int, GridBoardCell> cells { get; private set; }

		private void Start()
		{
			InitiateCells();
		}

		private void InitiateCells()
		{
			// Destroy previous cells, if they exist
			if (cells != null && cells.Count > 0)
				foreach (GridBoardCell cell in cells.Values)
					Destroy(cell);
			
			cells = new Dictionary<Vector2Int, GridBoardCell>();
			
			Vector3 startPosition = transform.position;
			Quaternion rotation = transform.rotation;
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					Vector2Int coordinates = new Vector2Int(i, j);
					GridBoardCell spawnedCell = Instantiate(_cellPrefab,
						startPosition + new Vector3(_spaceBetweenCells * i, 0, _spaceBetweenCells * j), rotation,
						transform);
					spawnedCell.Instantiate(this, coordinates);
					cells.Add(coordinates, spawnedCell);
				}
			}
		}
	}
}