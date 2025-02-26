using UnityEngine;

namespace _Prototyping.Utilities
{
	public class TransformLinker : MonoBehaviour
	{
		[SerializeField]
		private Transform _sourceTransform;

		[SerializeField]
		private bool _position = false;
		
		[SerializeField]
		private bool _rotation = true;

		private void Update()
		{
			if (_position)
				transform.position = _sourceTransform.position;
			
			if (_rotation)
				transform.rotation = _sourceTransform.rotation;
		}
	}
}