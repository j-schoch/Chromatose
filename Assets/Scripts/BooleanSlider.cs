using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BooleanSlider : MonoBehaviour
{
	[Serializable] public class UnityEvent_Bool : UnityEvent<bool>{}

	public UnityEvent OnValueTrue;
	public UnityEvent OnValueFalse;
	public UnityEvent_Bool OnValueChanged;
	public bool inverted;

	public Slider Slider { get; private set; }

	private void Awake()
	{
		Slider = GetComponent<Slider>();
		Slider.onValueChanged.AddListener((float value) => 
		{
			int intValue = Mathf.RoundToInt(value);
			intValue = inverted ? (1 - intValue) : intValue;
			bool boolValue = Convert.ToBoolean(intValue);

			if (boolValue)
			{
				OnValueTrue.Invoke();
			}
			else
			{
				OnValueFalse.Invoke();
			}

			OnValueChanged.Invoke(boolValue);
		});
	}
}
