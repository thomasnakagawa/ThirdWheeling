using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
	private static AudioSource audioSource;

	// Use this for initialization
	void Start () {
		audioSource = GetComponent<AudioSource> ();
	}

	public static void PlaySound(AudioClip audioClip) {
		audioSource.PlayOneShot (audioClip);
	}

	public static void PlaySound(AudioClip audioClip, float volume) {
		audioSource.PlayOneShot (audioClip, Mathf.Clamp(volume, 0f, 1f));
	}
}
