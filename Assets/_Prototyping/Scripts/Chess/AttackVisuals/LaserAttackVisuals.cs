using _Prototyping.Utilities;
using UnityEngine;

namespace _Prototyping.Chess.AttackVisuals
{
	public class LaserAttackVisuals : MonoBehaviour
	{
		[SerializeField]
		private Vector3 _positionOffsetFromCell;
		
		[SerializeField]
		private LineRenderer _lineRenderer;

		[SerializeField]
		private float _duration;

		[SerializeField]
		private AnimationCurve _widthOverDuration;

		[SerializeField]
		private Gradient _startColour;

		[SerializeField]
		private Gradient _endColour;
		
		private Vector3 _startPosition;
		private Vector3 _endPosition;
		private bool _initialized;
		private float _elapsedDuration;

		public float elapsedDurationNormalized => _elapsedDuration / _duration;

		public void Initialize(ChessBoardCell startCell, ChessBoardCell endCell, Vector2Int direction)
		{
			_startPosition = startCell.piecePositionTransform.position+_positionOffsetFromCell;
			if (endCell != null)
			{
				_endPosition = endCell.piecePositionTransform.position + _positionOffsetFromCell;
			}
			else
			{
				_endPosition = _startPosition + (new Vector3(direction.x, 0, direction.y) * 100);
			}

			_lineRenderer.positionCount = 2;
			_lineRenderer.SetPositions(new Vector3[]{_startPosition, _endPosition});

			_elapsedDuration = 0f;
			_initialized = true;
		}

		private void Update()
		{
			if (!_initialized)
				return;

			if (_elapsedDuration < _duration)
			{
				float lerpValue = _elapsedDuration / _duration;
				_lineRenderer.startWidth = _widthOverDuration.Evaluate(lerpValue);
				_lineRenderer.endWidth = _widthOverDuration.Evaluate(lerpValue);

				_lineRenderer.colorGradient = GradientHelpers.Lerp(_startColour, _endColour, lerpValue);
				
				_elapsedDuration += Time.deltaTime;
			}
			else
			{
				_lineRenderer.positionCount = 0;
				_elapsedDuration = _duration;
			}
		}
	}
}