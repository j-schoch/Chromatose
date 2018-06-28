using UnityEngine;
using DG.Tweening;

public class Rotate : MonoBehaviour
{
	[SerializeField] private float degreesPerSecond;
	[SerializeField] private bool randomize;
	[SerializeField] private Vector2 speedRange;

	private float randomDirection;
	private float randomSpeed;
	private float rotationRate;

	private void OnEnable()
	{
		rotationRate = degreesPerSecond;

		if (randomize)
		{
			randomDirection = (Random.Range(0, 2) * 2) - 1;
			randomSpeed = Random.Range(speedRange.x, speedRange.y);
			rotationRate = randomDirection * randomSpeed;
		}
	}

	private void Update()
	{
		transform.Rotate(Vector3.forward, rotationRate * Time.deltaTime);
	}
}
