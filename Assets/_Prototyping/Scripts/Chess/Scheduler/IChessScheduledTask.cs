using System;

namespace _Prototyping.Chess.Scheduler
{
	public interface IChessScheduledTask
	{
		public enum ScheduledTaskState
		{
			WaitingToStart,
			Executing,
			Finished
		}
		
		/// <summary>
		/// Which turn this task should be scheduled to start.
		/// </summary>
		public ChessManager.ChessGameState scheduledTurn { get; }
		
		public ScheduledTaskState taskState { get; }
		public Action<ScheduledTaskState> OnStateChanged { get; set; }

		/// <summary>
		/// Start the task, called once
		/// </summary>
		public void StartTask();

		/// <summary>
		/// Update the task, to be called each frame
		/// </summary>
		public void UpdateTask();

		/// <summary>
		/// End the task, cleaning up what is necessary, called once.
		/// </summary>
		public void EndTask();
	}
}