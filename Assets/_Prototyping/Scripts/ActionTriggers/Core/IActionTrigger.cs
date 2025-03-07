using System;
using System.Collections.Generic;

namespace _Prototyping.ActionTriggers.Core
{
	public interface IActionTrigger<T>
	{
		public List<ITriggerableAction<T>> triggerables { get; }
		public void AddAction(ITriggerableAction<T> triggerable);
		public void RemoveAction(ITriggerableAction<T> triggerable);
		
		public void TriggerActions(T triggerData);
		public Action<T> OnTriggered { get; set; }
	}
}