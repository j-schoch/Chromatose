using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour, IChargeable
{
	#region Inspector
	[SerializeField] private Vector2 speedRange;
	[SerializeField] private bool aimAtVelocity = true;
	#endregion Inspector

	public float Speed { get; set; }
	public Vector2 InheritedVelocity { get; set; }
	public Vector2 LaunchDirection { get; set; }

	public Rigidbody2D Rigidbody { get; private set; }
	public Rigidbody2D ParentRigidbody { get; set; }
	public Rotator Rotator { get; private set; }

	public List<ColorBall> ColorBalls { get; private set; }
	private int colorBallsAlive;

	private void Awake()
	{
		Rotator = GetComponentInChildren<Rotator>();
		Rigidbody = GetComponent<Rigidbody2D>();
		if(transform.parent != null)
		{
			ParentRigidbody = transform.parent.GetComponent<Rigidbody2D>();
		}
		ColorBalls = GetComponentsInChildren<ColorBall>().ToList();
		colorBallsAlive = ColorBalls.Count;
	}

	private void OnEnable()
	{
		foreach (ColorBall ball in ColorBalls)
		{
			ball.OnColorBallHit.AddListener(onColorBallHit);
		}

		Rigidbody.isKinematic = false;
		Rigidbody.simulated = true;
		Rigidbody.AddForce(InheritedVelocity, ForceMode2D.Impulse);
		Rigidbody.AddForce(LaunchDirection * Speed, ForceMode2D.Impulse);
	}

	private void Update()
	{
		if (aimAtVelocity)
		{
			transform.right = Rigidbody.velocity.normalized;
		}
	}

	private void onColorBallHit()
	{
		colorBallsAlive--;
		if (colorBallsAlive == 0)
		{
			Destroy(gameObject);
		}
	}

	private void killPhysics()
	{
		Rigidbody.simulated = false;
		Rigidbody2D[] children = GetComponentsInChildren<Rigidbody2D>();
		foreach (Rigidbody2D child in children)
		{
			child.simulated = false;
		}
	}

	void IChargeable.SetCharge(float charge)
	{
		Speed = Mathf.Lerp(speedRange.x, speedRange.y, charge);
	}

	void IChargeable.ResetCharge()
	{
		Speed = speedRange.x;
	}

	void IChargeable.OnChargeBegin() {}

	void IChargeable.OnChargeComplete(float finalCharge)
	{
		if (Rotator != null)
		{
			Rotator.enabled = true;
		}
		foreach(ColorBall colorball in ColorBalls)
		{
			colorball.Trail.widthMultiplier = finalCharge * colorball.EndScale.x;
		}
	}
}
