using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class SliderText : MonoBehaviour
{
	[SerializeField] Text textComponent;
	[SerializeField] Slider sliderComponent;
	[SerializeField] bool displayIndexAsCount;

	void OnValidate()
	{
		if (textComponent == null || sliderComponent == null)
		{
			Awake();
		}
	}

	void Awake()
	{
		textComponent = GetComponent<Text>();
		sliderComponent = GetComponentInParent<Slider>();
		if (textComponent == null || sliderComponent == null)
		{
			textComponent.text = sliderComponent.value.ToString();
		}
	}

	void OnEnable()
	{
		sliderComponent.onValueChanged.AddListener(onSliderValueChanged);
	}

	void OnDisable()
	{
		sliderComponent.onValueChanged.RemoveListener(onSliderValueChanged);
	}

	void onSliderValueChanged(float newValue)
	{
		if (displayIndexAsCount)
		{
			newValue += 1;
		}
		textComponent.text = newValue.ToString();
	}
}
