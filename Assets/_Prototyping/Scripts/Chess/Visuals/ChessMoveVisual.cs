using _Prototyping.Chess.Scheduler;
using _Prototyping.Chess.Scheduler.Tasks;
using _Prototyping.Utilities;
using UnityEngine;

namespace _Prototyping.Chess.Visuals
{
	public class ChessMoveVisual : MonoBehaviour
	{
		[SerializeField]
		private BezierLineRendererCurve _bezierLineRenderer;

		[SerializeField]
		private Vector3 _midPointOffset;
		
		private ChessPiece _piece;
		private PlayerMovePieceTask _task;

		private Vector3 _cachedPosition = Vector3.negativeInfinity;

		public void Setup(ChessPiece piece, PlayerMovePieceTask task)
		{
			_piece = piece;
			_task = task;
		}

		public void LateUpdate()
		{
			if (_piece == null || _task == null)
				return;

			if (_task.taskState == IChessScheduledTask.ScheduledTaskState.Finished)
			{
				Cleanup();
				return;
			}

			Vector3 piecePosition = _piece.tilePointTransform.position;
			if (piecePosition != _cachedPosition)
			{
				Vector3 endPosition = _task.cell.piecePositionTransform.position;
				Vector3 middlePosition = piecePosition + ((endPosition - piecePosition) / 2) + _midPointOffset;

				_bezierLineRenderer.DrawLine(piecePosition, middlePosition, endPosition);
			}
		}

		public void Cleanup()
		{
			Destroy(this.gameObject);
		}
	}
}