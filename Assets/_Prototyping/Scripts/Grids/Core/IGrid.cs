using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Grids.Core
{
	public interface IGrid<T> where T : IGridCell<T>
	{
		public Vector2Int initialDimensions { get; }
		public Dictionary<Vector2Int, T> cells { get; }
	}
}