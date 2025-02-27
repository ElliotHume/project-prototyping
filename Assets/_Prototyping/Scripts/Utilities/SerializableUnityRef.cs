using System;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;

namespace _Prototyping.Utilities
{
	/// <summary>
	/// The purpose of this class is to be able to serialize a reference to an object that derives from UnityEngine.Object in the inspector. 
	/// This is mostly useful for serializing an interface type instead of the concrete type.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	[Serializable]
	public class SerializableUnityRef<T> : ISerializableUnityRef where T : class
	{
		[SerializeField]
		private UnityObject _reference;

		[SerializeField]
		private T _value;

		public T value
		{
			get => _value == null ? _value = _reference as T : _value;
			set
			{
				bool changed = _value != value;

				_value = value;

				if (changed)
					_reference = _value as UnityObject;
			}
		}

		public bool hasValue => value != null;

		public Type valueType => typeof(T);

        internal void ValidateReferenceObject()
        {
	        if (_reference == null)
	        {
		        _value = null;
		        return;
	        }

	        if (valueType == typeof(GameObject) && _reference.GetType() == typeof(GameObject))
	        {
				return;
	        }

			if (_reference is GameObject go)
			{
				_reference = null;

				// find first component that inherits from T
				foreach (Component component in go.GetComponents<Component>())
				{
					if (component is T)
					{
						_reference = component;
						break;
					}
				}
			}
			else if (!(_reference is T))
			{
				_reference = null;
			}
		}

		public static T ValueOf(SerializableUnityRef<T> serializableUnityRef)
		{
			if (serializableUnityRef == null || serializableUnityRef._reference == null)
				return null;
			else
				return serializableUnityRef.value;
		}

        public static implicit operator T(SerializableUnityRef<T> serializableRef) => serializableRef.value;
		public static implicit operator SerializableUnityRef<T>(T val) => new SerializableUnityRef<T>() { value = val };

		/// <summary>
		/// If the referenced object is a Component, then get all components attached 
		/// to the same gameObject that are of type <typeparamref name="T"/>. <br />
		/// Otherwise, returns a single element list with just the reference object.
		/// </summary>
		/// <returns>
		/// A non-empty list of all components attached to the same gameObject as the 
		/// referenced object that are of type <typeparamref name="T"/>.
		/// </returns>
		public List<object> GetSiblingComponentsOfTypeFromReferenceObject()
		{
			List<object> siblingComponents = new List<object>();

			if (_reference == null)
				return siblingComponents;

			// we must cast the value and set the reference if value was set via code
			if (_reference == null && _value != null && _value is UnityObject valueAsUnityObject)
			{
				_reference = valueAsUnityObject;
            }

			if (_reference is Component component)
			{
				siblingComponents.AddRange(component.GetComponents<T>());
			}
			else if (!(_reference is T))
			{
				siblingComponents.Add(_reference);
			}

			return siblingComponents;
		}

		void ISerializationCallbackReceiver.OnBeforeSerialize()
		{
			ValidateReferenceObject();
		}

		void ISerializationCallbackReceiver.OnAfterDeserialize()
		{
			// do nothing
		}
    }
}
