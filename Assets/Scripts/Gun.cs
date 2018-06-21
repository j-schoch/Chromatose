using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Gun : MonoBehaviour
{
	[SerializeField] List<GameObject> projectiles;
	[SerializeField] float chargeDuration = 1f;
	[SerializeField] List<ColorRange> colorRangeToggles;

	bool MouseOverUI
	{
		get
		{
			return EventSystem.current.IsPointerOverGameObject();
		}
	}

	Rigidbody2D Rigidbody { get; set; }

	GameObject currentProjectilePrefab;
	GameObject queuedProjectile;
	Vector3 launchDirection;
	Vector3 mousePosition;
	bool firstShot = true;
	bool charging;
	float charge;
	float timeCharging;
	List<IChargeable> chargeables = new List<IChargeable>();

	int currentColorRange;
	int currentProjectileIndex = 0;
	void Awake()
	{
		Rigidbody = GetComponentInParent<Rigidbody2D>();
		ColorBall.ColorRange = colorRangeToggles[currentColorRange];
	}

	void Start()
	{
		if (projectiles.Count > 0)
		{
			currentProjectilePrefab = projectiles[0];
		}
		ResetCharging();
	}

	void Update()
	{
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

		if (Input.GetKeyDown(KeyCode.R))
		{
			currentColorRange++;
			if (currentColorRange >= colorRangeToggles.Count)
			{
				currentColorRange = 0;
			}

			ColorBall.ColorRange = colorRangeToggles[currentColorRange];
			SetProjectile(currentProjectileIndex);
		}

		if (!MouseOverUI && Input.GetKey(KeyCode.Mouse0))
		{
			if (!charging)
			{
				charging = true;
				foreach (IChargeable chargeable in chargeables)
				{
					chargeable.OnChargeBegin();
				}
			}
			else
			{
				UpdateLaunchDirection();
				timeCharging += Time.deltaTime;
				charge = Mathf.Clamp01(timeCharging / chargeDuration);
				foreach (IChargeable chargeable in chargeables)
				{
					chargeable.SetCharge(charge);
				}
			}
		}
		if (charging && Input.GetKeyUp(KeyCode.Mouse0))
		{
			if (firstShot)
			{
				firstShot = false;
				AudioManager.Instance.StartBackgroundMusic();
			}

			UpdateLaunchDirection();

			Bullet bullet = queuedProjectile.GetComponent<Bullet>();
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
