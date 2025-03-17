using System;
using System.Collections.Generic;

namespace _Prototyping.ActionTriggers.Core
{
	public interface IActionTrigger<T>
	{
		/// <summary>
		/// The list of TriggerableActions that listen to this ActionTrigger
		/// </summary>
		public List<ITriggerableAction<T>> triggerables { get; }
		/// <summary>
		/// Add a TriggerableAction to listen to this ActionTrigger
		/// </summary>
		/// <param name="triggerable">The TriggerableAction to add.</param>
		public void AddAction(ITriggerableAction<T> triggerable);
		/// <summary>
		/// Remove a TriggerableAction from this ActionTrigger, such that it no longer listens to it.
		/// </summary>
		/// <param name="triggerable">The TriggerableAction to remove.</param>
		public void RemoveAction(ITriggerableAction<T> triggerable);
		
		/// <summary>
		/// Activate the trigger.
		/// </summary>
		/// <param name="triggerData"></param>
		public void TriggerActions(T triggerData);
		public Action<T> OnTriggered { get; set; }
	}
}