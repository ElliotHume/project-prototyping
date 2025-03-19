using UnityEngine;

namespace _Prototyping.Utilities
{
	public static class BezierUtility
	{
		public static Vector3 CubicBezier(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
		{
			return (1f - t) * QuadraticBezier(p0, p1, p2, t) + t * QuadraticBezier(p1, p2, p3, t);
		}

		public static Vector3 QuadraticBezier(Vector3 p0, Vector3 p1, Vector3 p2, float t)
		{
			return (1f - t) * LinearBezier(p0, p1, t) + t * LinearBezier(p1, p2, t);
		}

		public static Vector3 LinearBezier(Vector3 p0, Vector3 p1, float t)
		{
			return (1f - t) * p0 + t * p1;
		}
	}
}