using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraExtras : MonoBehaviour
{
	public List<Color> BackgroundColors;

	public void SetBackgroundColor(int index)
	{
		index = Mathf.Clamp(index, 0, BackgroundColors.Count - 1);
		Camera.main.backgroundColor = BackgroundColors[index];
	}
}
