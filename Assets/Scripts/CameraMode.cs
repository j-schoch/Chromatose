using System.IO;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CameraMode : MonoBehaviour
{

	public static string SCREENSHOT_FOLDER = "/Screenshots/";
	public static string SCREENSHOT_EXT = ".png";

	[SerializeField] private Button pauseButton;
	[SerializeField] private Button playButton;
	[SerializeField] private Button screenShotButton;
	[SerializeField] private Button frameSkip;
	[SerializeField] private Ease timeChangeEase;
	[SerializeField] private float timeChangeDuration = 0.3f;


	private void OnEnable()
	{
		pauseButton.onClick.AddListener(Pause);
		playButton.onClick.AddListener(UnPause);
		screenShotButton.onClick.AddListener(TakeScreenshot);
		frameSkip.onClick.AddListener(SkipFrame);
	}

	private void OnDisable()
	{
		pauseButton.onClick.RemoveListener(Pause);
		playButton.onClick.RemoveListener(UnPause);
		screenShotButton.onClick.RemoveListener(TakeScreenshot);
		frameSkip.onClick.RemoveListener(SkipFrame);
	}

	private void Pause()
	{
		DOTween.Kill("SkipFrame");
		Time.timeScale = 0;
	}

	private void UnPause()
	{
		DOTween.Kill("SkipFrame");
		Time.timeScale = 1;
	}

	private void TakeScreenshot()
	{
		string path = Application.persistentDataPath + SCREENSHOT_FOLDER;
		int imageNumber = 0;

		if(!Directory.Exists(path))
		{
			Directory.CreateDirectory(path);
		}
		else
		{
			imageNumber = Directory.GetFiles(path, "*" + SCREENSHOT_EXT).Length;
		}
		ScreenCapture.CaptureScreenshot(path + imageNumber + SCREENSHOT_EXT);
	}

	private void SkipFrame()
	{
		DOTween.Kill("SkipFrame");
		Time.timeScale = 0;
		DOTween.To(() => Time.timeScale, f => Time.timeScale = f, 1f, timeChangeDuration)
			.SetLoops(2, LoopType.Yoyo)
			.SetEase(timeChangeEase)
			.SetId("SkipFrame")
			.SetUpdate(isIndependentUpdate: true);
	}
}
