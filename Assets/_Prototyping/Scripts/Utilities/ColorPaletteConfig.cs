using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace _Prototyping.Utilities
{
	[CreateAssetMenu(fileName = "ColorPalette", menuName = "PROTO/UI/Color Palette")]
	public class ColorPaletteConfig : ScriptableObject
	{
		public enum ColorName
		{
			BaseColour1 = 0,
			BaseColour2 = 1,
			
			AccentColour1 = 2,
			AccentColour2 = 3,
			AccentColour3 = 4,
			AccentColour4 = 5,
			AccentColour5 = 6,

			White = 98,
			Black = 99,
			Transparent = 100,

			FeedbackNeutral = 21,
			FeedbackHovered = 22,
			FeedbackSelected = 23,
			CallToAction = 24,
			StatusPositive = 25,
			StatusNegative = 26,
			ButtonNeutral = 27,
			ButtonHovered = 28,
			ButtonSelected = 29,
			ButtonLocked = 30,
		}
		
		public SerializedDictionary<ColorName, Color> colors = new SerializedDictionary<ColorName, Color>();
	}
}