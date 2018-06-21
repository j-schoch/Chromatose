using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockRotation : MonoBehaviour {

	public bool x;
	public bool y;
	public bool z;
	public Vector3 eulerAngles;

	public void LateUpdate () {
		if(x || y || z)
		{
			Vector3 newEulers = transform.eulerAngles;

			if (x)
			{
				newEulers.x = eulerAngles.x;
			}

			if (y)
			{
				newEulers.y = eulerAngles.y;
			}

			if (z)
			{
				newEulers.z = eulerAngles.z;
			}

			transform.eulerAngles = newEulers;
		}
	}
}
