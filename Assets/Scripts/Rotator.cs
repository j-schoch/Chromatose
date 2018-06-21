using UnityEngine;

public class Rotator : MonoBehaviour
{
	public float speed = 1f;

	void LateUpdate()
	{
		if (enabled)
		{
			transform.Rotate(0, 0, speed * Time.deltaTime);
		}
	}
}
