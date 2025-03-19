using UnityEngine;

namespace _Prototyping.Utilities
{
	[RequireComponent(typeof(LineRenderer))]
	[ExecuteAlways]
	public class BezierLineRendererCurve : MonoBehaviour
	{
		public LineRenderer lineRenderer;
		public int numberOfPoints = 15;
		public Transform startPointTransform;
		public Transform middlePointTransform;
		public Transform targetPointTransform;

		private bool _isActive;

		private void Reset()
		{
			lineRenderer = GetComponent<LineRenderer>();
		}

		public void ToggleIsActive(bool toggle)
		{
			_isActive = toggle;
			
			if (!_isActive)
				lineRenderer.positionCount = 0;
		}

		private void PositionBezierCurve()
		{
			Vector3 p0 = startPointTransform.position;
			Vector3 p1 = middlePointTransform.position;
			Vector3 p2 = targetPointTransform.position;

			lineRenderer.positionCount = numberOfPoints;
			for (int i = 0; i < numberOfPoints; i++)
			{
				lineRenderer.SetPosition(i, BezierUtility.QuadraticBezier(p0, p1, p2, (float)i / (numberOfPoints - 1)));
			}
		}

		public void DrawLine(Vector3 startPosition, Vector3 middlePosition, Vector3 endPosition)
		{
			startPointTransform.position = startPosition;
			middlePointTransform.position = middlePosition;
			targetPointTransform.position = endPosition;
			DrawLine();
		}

		public void DrawLine()
		{
			if (lineRenderer == null)
				return;

			if (_isActive && startPointTransform != null && middlePointTransform != null && targetPointTransform != null)
			{
				PositionBezierCurve();
			}
			else
			{
				lineRenderer.positionCount = 0;
			}
		}

#if UNITY_EDITOR
		private void Update()
		{
			_isActive = true;
			DrawLine();
		}
#endif
	}
}