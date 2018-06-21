using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomColorRange : MonoBehaviour {

	public static Color GetRandomColor(Vector2 hueRange, Vector2 saturationRange, Vector2 brightnessRange)
	{
		float h = Mathf.Clamp(Random.value, hueRange.x, hueRange.y);
		float s = Mathf.Clamp(Random.value, saturationRange.x, saturationRange.y);
		float v = Mathf.Clamp(Random.value, brightnessRange.x, brightnessRange.y);
		return Color.HSVToRGB(h, s, v);
	}
}
