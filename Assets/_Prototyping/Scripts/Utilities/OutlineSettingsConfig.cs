using UnityEngine;

namespace _Prototyping.Utilities
{
	[CreateAssetMenu(menuName = "PROTO/Outlines/" + nameof(OutlineSettingsConfig), fileName = nameof(OutlineSettingsConfig)+"_Default")]
	public class OutlineSettingsConfig : ScriptableObject
	{
		public Outline.Mode outlineMode = Outline.Mode.OutlineAll;
		public Color outlineColor = Color.white;
		public float outlineWidth = 1f;
	}
}