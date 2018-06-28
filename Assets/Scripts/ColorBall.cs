using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;

public class ColorBall : MonoBehaviour, IChargeable
{
	public static ColorRange ColorRange;

	public static bool LifetimeEnabled = true;

	[SerializeField] private Vector3 startScale;
	[SerializeField] private Vector3 endScale;
	[SerializeField] private GameObject decalPrefab;
	[SerializeField] private float lifetime = 30f;

	public UnityEvent OnColorBallHit;

	public Vector3 StartScale { get { return startScale; } private set { startScale = value; } }
	public Vector3 EndScale { get { return endScale; } private set { endScale = value; } }
	public TrailRenderer Trail { get; private set; }
	public SpriteRenderer Sprite { get; private set; }
	public Color Color { get; private set; }
	public Collider2D CollisionCircle { get; private set; }

	void Awake()
	{
		CollisionCircle = GetComponent<Collider2D>();
		Color = ColorRange.GetRandomColor();
		Sprite = GetComponent<SpriteRenderer>();
		Sprite.color = Color;

		Trail = GetComponent<TrailRenderer>();
		Trail.startColor = Color;
		Trail.endColor = Color;
		Trail.enabled = false;
	}

	private IEnumerator StopAfterLifetime()
	{
		float elapsedTime = 0;

		while (elapsedTime < lifetime)
		{
			while (!LifetimeEnabled)
			{
				yield return null;
			}
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		Stop();
	}

	public void Stop()
	{
		Trail.DOResize(0, 0, 5f)
			.OnUpdate(() => {
				Sprite.transform.localScale = Vector3.one * Trail.startWidth;
			})
			.OnComplete(() => {
				OnColorBallHit.Invoke();
				Destroy(gameObject);
			});
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if(other.gameObject.layer == 11)
		{
			return;
		}
		if (decalPrefab != null)
		{
			// spawn decal
			Vector3 surfacePosition = Physics2D.Distance(CollisionCircle, other).pointB;
			Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
			GameObject colorSpot = Instantiate(original: decalPrefab, position: surfacePosition, rotation: randomRotation, parent: other.transform);

			colorSpot.transform.localScale *= Trail.startWidth * 0.5f;

			// set color
			SpriteRenderer spotRenderer = colorSpot.GetComponent<SpriteRenderer>();
			Color color = Trail.startColor;
			color.a = spotRenderer.color.a;
			spotRenderer.color = color;

			SpriteRenderer otherRenderer = other.gameObject.GetComponentInChildren<SpriteRenderer>();
			spotRenderer.sortingLayerID = otherRenderer.sortingLayerID;
			spotRenderer.sortingOrder = otherRenderer.sortingOrder + 1;

			transform.SetParent(null);
		}

		OnColorBallHit.Invoke();
	}

	void IChargeable.SetCharge(float charge)
	{
		transform.localScale = Vector3.Lerp(StartScale, endScale, charge);
	}

	void IChargeable.ResetCharge()
	{
		transform.localScale = StartScale;
	}

	void IChargeable.OnChargeBegin()
	{
	}

	void IChargeable.OnChargeComplete(float finalCharge)
	{
		Trail.enabled = true;
		Trail.widthMultiplier = Vector3.Lerp(StartScale, EndScale, finalCharge).x;
		Sprite.enabled = false;
		CollisionCircle.isTrigger = true;
		StartCoroutine(StopAfterLifetime());
	}
}
