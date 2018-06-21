using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SupportButton : MonoBehaviour {

	[SerializeField] private Button yesButton;

	private void Awake()
	{
		yesButton.onClick.AddListener(this.ShowAd);
	}

	private void ShowAd()
	{
		AdManager.Instance.ShowAd();
	}
}
