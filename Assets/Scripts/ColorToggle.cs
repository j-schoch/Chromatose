using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorToggle : MonoBehaviour {

	public Color colorA;
	public Color colorB;
	public bool isA;

	private SpriteRenderer spriteRenderer;
	public SpriteRenderer SpriteRenderer
	{
		get
		{
			if(spriteRenderer == null)
			{
				spriteRenderer = GetComponent<SpriteRenderer>();
			}
			return spriteRenderer;
		}
	}

	public void Toggle(bool setA)
	{
		SpriteRenderer.color = setA ? colorA : colorB;
		isA = setA;
	}

	public void Toggle()
	{
		Toggle(!isA);
	}

	private void OnValidate()
	{
		if (SpriteRenderer != null)
		{
			Toggle(isA);
		}
	}

}
