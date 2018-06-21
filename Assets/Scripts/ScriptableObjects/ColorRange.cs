using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ColorRange", menuName = "Config/ColorRange")]
public class ColorRange : ScriptableObject
{
	[Header("Random Within Range")]
	[SerializeField] Vector2 hueRange;
	[SerializeField] Vector2 saturationRange;
	[SerializeField] Vector2 brightnessRange;
	[Header("Manual Color Definition")]
	[SerializeField] bool useDefinedColorChoices;
	[SerializeField] List<Color> definedColors;

	public Color GetRandomColor()
	{
		if (useDefinedColorChoices)
		{
			return definedColors[Random.Range(0, definedColors.Count)];
		}
		else
		{
			float h = Mathf.Clamp(Random.value, hueRange.x, hueRange.y);
			float s = Mathf.Clamp(Random.value, saturationRange.x, saturationRange.y);
			float v = Mathf.Clamp(Random.value, brightnessRange.x, brightnessRange.y);
			return Color.HSVToRGB(h, s, v);
		}
	}
}