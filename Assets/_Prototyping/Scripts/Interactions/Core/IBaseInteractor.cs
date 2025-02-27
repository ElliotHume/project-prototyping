namespace _Prototyping.Interactions.Core
{
	public interface IBaseInteractor
	{
		public bool isInteracting { get; }
		public bool isHovering { get; }

		public bool isLowPriority { get; }

		/// <summary>
		/// 
		/// </summary>
		/// <returns>A boolean value for if this interaction should block others from taking place.</returns>
		public bool OnInteractInputPressed();
	}
}