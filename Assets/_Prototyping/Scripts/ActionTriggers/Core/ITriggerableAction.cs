using System;

namespace _Prototyping.ActionTriggers.Core
{
	public interface ITriggerableAction<T>
	{
		public Action<T> OnActionTriggered { get; set; }
		
		/// <summary>
		/// Activate the action.
		/// </summary>
		/// <param name="triggerData"></param>
		public void Trigger(T triggerData);
	}
}