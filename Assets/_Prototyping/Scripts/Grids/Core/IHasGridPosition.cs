namespace _Prototyping.Grids.Core
{
	public interface IHasGridPosition<T> where T : IGridCell<T>
	{
		public IGridCell<T> Cell { get; }
	}
}