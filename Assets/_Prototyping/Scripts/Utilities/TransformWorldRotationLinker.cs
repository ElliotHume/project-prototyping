using UnityEngine;

namespace _Prototyping.Utilities
{
	public class TransformWorldRotationLinker : MonoBehaviour
	{
		[SerializeField]
		private Transform _sourceRotationTransform;

		private void Update()
		{
			transform.rotation = _sourceRotationTransform.rotation;
		}
	}
}