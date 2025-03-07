using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Chess.Scheduler
{
	public class ChessScheduler : MonoBehaviour
	{
		public static ChessScheduler Instance;
		
		public Action<IChessScheduledTask> OnTaskFinished;
		
		private Dictionary<ChessManager.ChessGameState, Queue<IChessScheduledTask>> _turnQueues;

		private void Awake()
		{
			ChessScheduler.Instance = this;
			_turnQueues = new Dictionary<ChessManager.ChessGameState, Queue<IChessScheduledTask>>();
			_turnQueues.Add(ChessManager.ChessGameState.PlayerTurn, new Queue<IChessScheduledTask>());
			_turnQueues.Add(ChessManager.ChessGameState.ResolvingPlayerTurn, new Queue<IChessScheduledTask>());
			_turnQueues.Add(ChessManager.ChessGameState.EnemyTurn, new Queue<IChessScheduledTask>());
			_turnQueues.Add(ChessManager.ChessGameState.ResolvingEnemyTurn, new Queue<IChessScheduledTask>());
		}

		public void AddScheduledTask(ChessManager.ChessGameState turnToRun, IChessScheduledTask task)
		{
			if (_turnQueues.TryGetValue(turnToRun, out Queue<IChessScheduledTask> queue))
			{
				queue.Enqueue(task);
			}
		}

		public void UpdateTasks(ChessManager.ChessGameState currentGameState)
		{
			Queue<IChessScheduledTask> queue = _turnQueues[currentGameState];
			if (queue.Count == 0)
				return;
			
			IChessScheduledTask task = queue.Peek();
			switch (task.taskState)
			{
				case IChessScheduledTask.ScheduledTaskState.WaitingToStart:
					task.StartTask();
					break;
				case IChessScheduledTask.ScheduledTaskState.Executing:
					task.UpdateTask();
					break;
				case IChessScheduledTask.ScheduledTaskState.Finished:
					IChessScheduledTask finishedTask = queue.Dequeue();
					finishedTask.EndTask();
					OnTaskFinished?.Invoke(finishedTask);
					break;
			}
		}

		public bool CheckCanMoveToNextGameState(ChessManager.ChessGameState currentGameState)
		{
			return _turnQueues[currentGameState].Count == 0;
		}
	}
}