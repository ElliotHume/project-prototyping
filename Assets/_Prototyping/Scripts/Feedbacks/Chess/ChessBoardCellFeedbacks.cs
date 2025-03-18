using _Prototyping.Chess;
using _Prototyping.PointerSelectables;
using AYellowpaper.SerializedCollections;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace _Prototyping.Feedbacks.Chess
{
	public class ChessBoardCellFeedbacks : MonoBehaviour
	{
		public enum ChessBoardCellFeedbackType
		{
			StartHover,
			EndHover,
			StartSelection,
			EndSelection,
			PieceMovedOntoCell,
			PieceMovedOffOfCell,
		}

		[SerializeField]
		private ChessBoardCell _chessBoardCell;

		[SerializeField]
		private ChessBoardCellSelectable _chessBoardCellSelectable;

		[SerializeField]
		private SerializedDictionary<ChessBoardCellFeedbackType, MMF_Player> _feedbacks;

		private void Reset()
		{
			_chessBoardCell = GetComponentInParent<ChessBoardCell>();
			_chessBoardCellSelectable = GetComponentInParent<ChessBoardCellSelectable>();
		}

		private void Start()
		{
			_chessBoardCell.OnPieceMovedToCellUnityEvent.AddListener(OnPieceChanged);
			
			_chessBoardCellSelectable.OnHoverStartUnityEvent.AddListener(OnPieceHoverStarted);
			_chessBoardCellSelectable.OnHoverEndUnityEvent.AddListener(OnPieceHoverEnded);
			_chessBoardCellSelectable.OnSelectionStartUnityEvent.AddListener(OnPieceSelectStarted);
			_chessBoardCellSelectable.OnSelectionEndUnityEvent.AddListener(OnPieceSelectEnded);
		}
		
		private void OnDestroy()
		{
			_chessBoardCell.OnPieceMovedToCellUnityEvent.RemoveListener(OnPieceChanged);
			
			_chessBoardCellSelectable.OnHoverStartUnityEvent.RemoveListener(OnPieceHoverStarted);
			_chessBoardCellSelectable.OnHoverEndUnityEvent.RemoveListener(OnPieceHoverEnded);
			_chessBoardCellSelectable.OnSelectionStartUnityEvent.RemoveListener(OnPieceSelectStarted);
			_chessBoardCellSelectable.OnSelectionEndUnityEvent.RemoveListener(OnPieceSelectEnded);
		}

		private void OnPieceChanged(ChessPiece piece)
		{
			if (piece != null)
			{
				PlayFeedback(ChessBoardCellFeedbackType.PieceMovedOntoCell);
			}
			else
			{
				PlayFeedback(ChessBoardCellFeedbackType.PieceMovedOffOfCell);
			}
		}
		
		private void OnPieceHoverStarted()
		{
			PlayFeedback(ChessBoardCellFeedbackType.StartHover);
		}
		
		private void OnPieceHoverEnded()
		{
			PlayFeedback(ChessBoardCellFeedbackType.EndHover);
		}

		private void OnPieceSelectStarted()
		{
			PlayFeedback(ChessBoardCellFeedbackType.StartSelection);
		}
		
		private void OnPieceSelectEnded()
		{
			PlayFeedback(ChessBoardCellFeedbackType.EndSelection);
		}

		private void PlayFeedback(ChessBoardCellFeedbackType feedbackType)
		{
			if (_feedbacks.TryGetValue(feedbackType, out MMF_Player feedback))
				feedback.PlayFeedbacks();
		}
	}
}