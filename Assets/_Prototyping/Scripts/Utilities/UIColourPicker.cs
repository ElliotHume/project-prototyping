using UnityEngine;
using UnityEngine.UI;

namespace _Prototyping.Utilities
{
	[RequireComponent(typeof(Image))]
#if UNITY_EDITOR
	[ExecuteInEditMode]
#endif
	public class UIColourPicker : MonoBehaviour
	{
		[SerializeField]
		private ColorPaletteConfig _colorPalette;

		[SerializeField]
		private ColorPaletteConfig.ColorName _color;

		private Image _image;

#if UNITY_EDITOR
		private void Update()
		{
			if (_image == null)
				_image = GetComponent<Image>();

			if (_image != null && _colorPalette != null)
				_image.color = _colorPalette.colors[_color];
		}
#endif
	}
}