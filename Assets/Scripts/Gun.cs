using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
	[SerializeField] private List<GameObject> projectiles;
	[SerializeField] private float chargeDuration = 1f;
	[SerializeField] private List<ColorRange> colorRangeToggles;

	private bool MouseOverUI
	{
		get
		{
			return EventSystem.current.IsPointerOverGameObject();
		}
	}

	private Rigidbody2D Rigidbody { get; set; }

	private GameObject currentProjectilePrefab;
	private GameObject queuedProjectile;
	private Vector3 launchDirection;
	private Vector3 mousePosition;
	private bool firstShot = true;
	private bool charging;
	private float charge;
	private float timeCharging;
	private List<IChargeable> chargeables = new List<IChargeable>();

	private int currentColorList;
	private int currentProjectileIndex = 0;

	private List<ColorBall> colorBalls = new List<ColorBall>();

	void Awake()
	{
		Rigidbody = GetComponentInParent<Rigidbody2D>();
		ColorBall.ColorRange = colorRangeToggles[currentColorList];
	}

	private void Start()
	{
		if (projectiles.Count > 0)
		{
			currentProjectilePrefab = projectiles[0];
		}
		ResetCharging();
	}

	private void Update()
	{
		if (Time.timeScale < 1)
		{
			return;
		}

		if (Input.GetKeyDown(KeyCode.Alpha1))
		{
			SetProjectile(0);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha2))
		{
			SetProjectile(1);
		}
		else if (Input.GetKeyDown(KeyCode.Alpha3))
		{
			SetProjectile(2);
		}

		if (!MouseOverUI && Input.GetKeyDown(KeyCode.Mouse0))
		{
			charging = true;
			foreach (IChargeable chargeable in chargeables)
			{
				chargeable.OnChargeBegin();
			}
		}

		if (charging)
		{
			if (Input.GetKey(KeyCode.Mouse0))
			{
				UpdateLaunchDirection();
				timeCharging += Time.deltaTime;
				charge = Mathf.Clamp01(timeCharging / chargeDuration);
				foreach (IChargeable chargeable in chargeables)
				{
					chargeable.SetCharge(charge);
				}
			}

			if (Input.GetKeyUp(KeyCode.Mouse0))
			{
				if (firstShot)
				{
					firstShot = false;
					AudioManager.Instance.StartBackgroundMusic();
				}

				UpdateLaunchDirection();

				Bullet bullet = queuedProjectile.GetComponent<Bullet>();
				colorBalls.AddRange(bullet.ColorBalls);
				bullet.LaunchDirection = launchDirection;
				bullet.InheritedVelocity = Rigidbody.velocity * 0.3f;
				if (bullet.Rotator != null)
				{
					bullet.Rotator.speed = Rigidbody.angularVelocity;
				}
				bullet.transform.SetParent(null);
				bullet.enabled = true;

				foreach (IChargeable chargeable in chargeables)
				{
					chargeable.OnChargeComplete(charge);
				}

				ResetCharging();
			}
		}
	}

	public void SetLifetimeEnabled(bool enabled)
	{
		ColorBall.LifetimeEnabled = enabled;
	}

	public void TimeoutAllProjectiles()
	{
		foreach (ColorBall cb in colorBalls)
		{
			if (cb != null)
			{
				cb.Stop();
			}
		}
		colorBalls.Clear();
	}

	public void SetProjectileColor(int index)
	{
		if (ColorBall.ColorRange != colorRangeToggles[index])
		{
			ColorBall.ColorRange = colorRangeToggles[index];
			SetProjectile(currentProjectileIndex);
		}
	}

	public void SetProjectile(int index)
	{
		if (index < projectiles.Count)
		{
			currentProjectileIndex = index;
			currentProjectilePrefab = projectiles[index];
			if(queuedProjectile != null)
			{
				DestroyImmediate(queuedProjectile);
			}
			ResetCharging();
		}
	}
			
	private void UpdateLaunchDirection()
	{
		mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		mousePosition.z = transform.position.z;
		launchDirection = (mousePosition - transform.position).normalized;
	}

	void ResetCharging()
	{
		charge = 0;
		timeCharging = 0;
		charging = false;

		queuedProjectile = Instantiate(currentProjectilePrefab, transform.position, transform.rotation, transform);
		chargeables.Clear();
		chargeables = queuedProjectile.GetComponentsInChildren<IChargeable>().ToList();
	}
}
