using System;
using UnityEngine;

namespace _Prototyping.Utilities
{
	/// <summary>
	/// Disable the GUI for a property.
	/// </summary>
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class DisabledAttribute : PropertyAttribute
	{
		//
	}
}