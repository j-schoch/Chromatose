using UnityEngine;
using System.Collections;

public class SmoothFollow2D : MonoBehaviour
{
	public Transform target;
	public float smoothTime;
	public float rotationSpeed;

	private GameObject lastNearestGround;

	// Update is called once per frame
	void LateUpdate()
	{
		if (target != null)
		{
			Roller roller = target.GetComponent<Roller>();
			if (roller.NearestGround != null)
			{
				lastNearestGround = roller.NearestGround;
				Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, target.position - lastNearestGround.transform.position);

				transform.rotation = newRotation;
			}

		}

	}
}