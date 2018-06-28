using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BooleanSliderGroup : MonoBehaviour
{
	public List<Slider> sliders;
	public bool thereCanOnlyBeOne;
	public bool allowAllOff;

	public void OnEnable()
	{
		foreach(Slider slider in sliders)
		{
			slider.onValueChanged.AddListener((float f) =>
			{
				SliderValueChanged(slider, f);
			});
		}
	}

	public void OnDisable()
	{
		foreach(Slider slider in sliders)
		{
			slider.onValueChanged.RemoveAllListeners();
		}
	}

	private void SliderValueChanged(Slider slider, float newValue)
	{
		if(newValue == 1)
		{
			foreach (Slider s in sliders)
			{
				if (s != slider)
				{
					if (thereCanOnlyBeOne && s.value == 1)
					{
						s.value = 0;
					}
				}
			}
		}
		else
		{
			if (!allowAllOff)
			{
				foreach (Slider s in sliders)
				{
					bool anotherSliderIsOn = s != slider && s.value == 1;
					if (anotherSliderIsOn)
					{
						return;
					}
				}

				slider.value = 1;
			}
		}
		
	}
}
