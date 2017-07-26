using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

	public static SoundManager instance = null;

	public AudioSource effectsSource;
	public AudioSource musicSource;
	public float pitchMin = 0.95f;
	public float pitchMax = 1.05f;

	void Awake() {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);
	}

	public void PlaySingleSfx(AudioClip clip) {
		effectsSource.clip = clip;
		effectsSource.Play ();
	}

	public void RandomizeSfx(params AudioClip[] clips) {
		int clipIndex = Random.Range (0, clips.Length);
		float pitch = Random.Range (pitchMin, pitchMax);
		effectsSource.pitch = pitch;
		effectsSource.clip = clips [clipIndex];
		effectsSource.Play ();
	}
}
