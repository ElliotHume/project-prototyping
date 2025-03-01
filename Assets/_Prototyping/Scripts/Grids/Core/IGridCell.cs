using UnityEngine;

namespace _Prototyping.Grids.Core
{
	public interface IGridCell<T> where T : IGridCell<T>
	{
		public IGrid<T> grid { get; }
		public Vector2Int gridCoordinates { get; }
	}
}