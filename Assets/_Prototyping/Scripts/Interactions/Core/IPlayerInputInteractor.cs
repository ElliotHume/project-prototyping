namespace _Prototyping.Interactions.Core
{
	public interface IPlayerInteractInputReceiver
	{
		/// <returns>A boolean value for if this interaction should block others from taking place.</returns>
		public bool OnInteractInputPressed();

		/// <returns>A boolean value for if this interaction should block others from taking place.</returns>
		public bool OnInteractInputHeld(float heldDuration);
		
		public void OnInteractInputReleased();
	}
}