using System;

namespace _Prototyping.ActionTriggers.Core
{
	public interface ITriggerableAction<T>
	{
		public Action<T> OnActionTriggered { get; set; }
		public void Trigger(T triggerData);
	}
}