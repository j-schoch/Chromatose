using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviourSingleton<AudioManager>
{
	public AudioClip backgroundMusic;
	private AudioSource source;

	private void Awake()
	{
		source = GetComponent<AudioSource>();
	}


	public void StartBackgroundMusic()
	{
		if (enabled)
		{
			source.clip = backgroundMusic;
			source.Play();
		}
	}
}
