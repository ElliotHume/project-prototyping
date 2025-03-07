using System;

namespace _Prototyping.ActionTriggers.Core
{
	public interface ITriggerableAction<T>
	{
		public void Trigger(T triggerData);
		public Action<IActionTrigger<T>> OnActionTriggered { get; set; }
	}
}