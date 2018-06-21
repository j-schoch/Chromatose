using System.Collections;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdManager : MonoBehaviourSingleton<AdManager> {
	public void ShowAd()
	{
		StartCoroutine(TryShowAd());
	}

	private IEnumerator TryShowAd()
	{
		if (Advertisement.IsReady())
		{
			Advertisement.Show();
			yield break;
		}

		// Ads are not initialized yet, wait a little and try again
		yield return new WaitForSeconds(1f);

		if (Advertisement.IsReady())
		{
			Advertisement.Show();
			yield break;
		}

		Debug.LogError("Something went wrong");
	}
}
