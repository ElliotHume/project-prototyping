namespace _Prototyping.Interactions.Core
{
	public interface IBaseInteractor
	{
		public bool isInteracting { get; }
		public bool isHovering { get; }
		public int priority { get; }
	}
}