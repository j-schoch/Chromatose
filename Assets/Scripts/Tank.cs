using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tank : MonoBehaviour {

	public Transform gun;
	public GameObject projectile;
	[SerializeField] public Vector2 centerOfMassOffset;
	[SerializeField] private float _speed;


	private Rigidbody2D _rigidbody2D;
	public Rigidbody2D Rigidbody2D
	{
		get
		{
			if (_rigidbody2D == null)
			{
				_rigidbody2D = GetComponent<Rigidbody2D>();
			}
			return _rigidbody2D;
		}
	}

	public float Speed
	{
		get
		{
			return _speed;
		}
		set
		{
			if(value != _speed)
			{
				_speed = value;
			}
		}
	}

	private Animator _animator;
	public Animator Animator
	{
		get
		{
			if(_animator == null)
			{
				_animator = GetComponent<Animator>();
			}
			return _animator;
		}
	}

	private WheelJoint2D[] wheels;

	private Vector2 input;

	private void Awake()
	{
		wheels = GetComponentsInChildren<WheelJoint2D>();
		Rigidbody2D.centerOfMass += centerOfMassOffset;
	}

	private void Update()
	{
		input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

		if (input.x != 0)
		{
			Vector3 newScale = transform.localScale;
			newScale.x = Mathf.Abs(transform.localScale.x) * Mathf.Sign(input.x);
			transform.localScale = newScale;
		}

		if(input.y != 0)
		{
			Vector3 eulers = gun.localEulerAngles;
			eulers.z += input.y;
			eulers.z = Mathf.Clamp(eulers.z, 0, 90);
			gun.localEulerAngles = eulers;
		}

		if (Input.GetKeyDown(KeyCode.Space))
		{
			FireProjectile();
		}

		foreach(WheelJoint2D wheel in wheels)
		{
			JointMotor2D motor = wheel.motor;
			motor.motorSpeed = input.x * (-Speed * 100) * Time.deltaTime;
			wheel.motor = motor;
		}
	}

	private void FireProjectile()
	{
	}
}
