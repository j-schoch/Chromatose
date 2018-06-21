using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roller : MonoBehaviour
{
	[SerializeField] private float speed;
	[SerializeField] private float maxSpeed;
	[SerializeField] private float jumpForce;
	[SerializeField] private LayerMask groundLayers;

	public Vector2 GroundNormal { get; private set; }
	public GameObject NearestGround { get; private set; }

	public Collider2D Collider { get; private set; }
	public Rigidbody2D Rigidbody { get; private set; }

	private Vector3 Input { get; set; }
	private bool bShouldJump;
	private bool bGrounded;

	// Use this for initialization
	private void Start()
	{
		Rigidbody = GetComponentInChildren<Rigidbody2D>();
		Collider = GetComponentInChildren<Collider2D>();
	}

	// Update is called once per frame
	private void Update()
	{
		Input = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
		if (bGrounded && UnityEngine.Input.GetKeyDown(KeyCode.Space))
		{
			bShouldJump = true;
		}
	}

	private void FixedUpdate()
	{
		ContactFilter2D filter = new ContactFilter2D();
		filter.useLayerMask = true;
		filter.layerMask = groundLayers;

		Collider2D[] results = new Collider2D[1];
		Collider.OverlapCollider(filter, results);
		if (results.Length > 0 && results[0] != null)
		{
			ColliderDistance2D distance2D = Physics2D.Distance(results[0], Collider);
			GroundNormal = distance2D.normal;
			NearestGround = results[0].gameObject;
			if (bShouldJump)
			{
				Rigidbody.AddForce(distance2D.normal * jumpForce, ForceMode2D.Impulse);
				bShouldJump = false;
				bGrounded = false;
			}
		}
		else
		{
			GroundNormal = Vector2.zero;
		}

		if (!bGrounded && Collider.IsTouchingLayers(groundLayers))
		{
			bGrounded = true;
		}


		if (Input.x != 0)
		{
			Rigidbody.AddTorque(Input.x * speed * Time.deltaTime);
		}
	}
}
