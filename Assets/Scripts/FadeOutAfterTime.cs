using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutAfterTime : MonoBehaviour
{
	public float delay;
	public float fadeTime;

	private SpriteRenderer spriteRenderer;

	void Awake ()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();
		StartCoroutine(fadeOutAfterTime());
	}

	IEnumerator fadeOutAfterTime()
	{
		yield return new WaitForSeconds(delay);

		float elapsed = 0;
		float progress = 0;

		Color startcolor = spriteRenderer.color;
		Color endColor = spriteRenderer.color;
		endColor.a = 0;

		while(progress <= 1)
		{
			elapsed += Time.deltaTime;
			progress = Mathf.Clamp01(elapsed / fadeTime);
			spriteRenderer.color = Color.Lerp(startcolor, endColor, progress);
			yield return null;
		}

		Destroy(gameObject);
	}
}
