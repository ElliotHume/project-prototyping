using System;
using System.Collections.Generic;
using UnityEngine;

namespace _Prototyping.Utilities
{
	public interface ISerializableUnityRef : ISerializationCallbackReceiver
	{
		public List<object> GetSiblingComponentsOfTypeFromReferenceObject();

		public Type valueType { get; }
	}
}