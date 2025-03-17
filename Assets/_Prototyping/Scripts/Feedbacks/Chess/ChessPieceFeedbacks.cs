using _Prototyping.Chess;
using _Prototyping.PointerSelectables;
using AYellowpaper.SerializedCollections;
using MoreMountains.Feedbacks;
using UnityEngine;

namespace _Prototyping.Feedbacks.Chess
{
	public class ChessPieceFeedbacks : MonoBehaviour
	{
		public enum ChessPieceFeedbackType
		{
			StartHover,
			EndHover,
			StartSelection,
			EndSelection,
			Move,
			TakeOtherPiece,
			Die,
		}
		
		[SerializeField]
		private ChessPiece _chessPiece;

		[SerializeField]
		private ChessPieceSelectable _chessPieceSelectable;

		[SerializeField]
		private SerializedDictionary<ChessPieceFeedbackType, MMF_Player> _feedbacks;

		private void Reset()
		{
			_chessPiece = GetComponentInParent<ChessPiece>();
			_chessPieceSelectable = GetComponentInParent<ChessPieceSelectable>();
		}

		private void Start()
		{
			_chessPiece.OnChangedCellsUnityEvent.AddListener(OnPieceMoved);
			_chessPiece.OnPieceTakeOther += OnPieceTakeOther;
			_chessPiece.OnThisPieceTaken += OnThisPieceTaken;
			
			_chessPieceSelectable.OnHoverStartUnityEvent.AddListener(OnPieceHoverStarted);
			_chessPieceSelectable.OnHoverEndUnityEvent.AddListener(OnPieceHoverEnded);
			_chessPieceSelectable.OnSelectionStartUnityEvent.AddListener(OnPieceSelectStarted);
			_chessPieceSelectable.OnSelectionEndUnityEvent.AddListener(OnPieceSelectEnded);
		}
		
		private void OnDestroy()
		{
			_chessPiece.OnChangedCellsUnityEvent.RemoveListener(OnPieceMoved);
			_chessPiece.OnPieceTakeOther -= OnPieceTakeOther;
			_chessPiece.OnThisPieceTaken -= OnThisPieceTaken;
			
			_chessPieceSelectable.OnHoverStartUnityEvent.RemoveListener(OnPieceHoverStarted);
			_chessPieceSelectable.OnHoverEndUnityEvent.RemoveListener(OnPieceHoverEnded);
			_chessPieceSelectable.OnSelectionStartUnityEvent.RemoveListener(OnPieceSelectStarted);
			_chessPieceSelectable.OnSelectionEndUnityEvent.RemoveListener(OnPieceSelectEnded);
		}

		private void OnPieceHoverStarted()
		{
			PlayFeedback(ChessPieceFeedbackType.StartHover);
		}
		
		private void OnPieceHoverEnded()
		{
			PlayFeedback(ChessPieceFeedbackType.EndHover);
		}

		private void OnPieceSelectStarted()
		{
			PlayFeedback(ChessPieceFeedbackType.StartSelection);
		}
		
		private void OnPieceSelectEnded()
		{
			PlayFeedback(ChessPieceFeedbackType.EndSelection);
		}

		private void OnPieceMoved(ChessBoardCell cell)
		{
			PlayFeedback(ChessPieceFeedbackType.Move);
		}
		
		private void OnPieceTakeOther(ChessPiece obj)
		{
			PlayFeedback(ChessPieceFeedbackType.TakeOtherPiece);
		}
		
		private void OnThisPieceTaken(ChessPiece obj)
		{
			PlayFeedback(ChessPieceFeedbackType.Die);
		}

		private void PlayFeedback(ChessPieceFeedbackType feedbackType)
		{
			if (_feedbacks.TryGetValue(feedbackType, out MMF_Player feedback))
				feedback.PlayFeedbacks();
		}
	}
}