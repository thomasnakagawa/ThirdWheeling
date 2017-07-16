using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wheel : MonoBehaviour {
	[SerializeField] private AudioClip hitSound;
	[SerializeField] private float minVel = 2f;
	[SerializeField] private float maxVel = 5f;
	[SerializeField] private AnimationCurve volumeCurve;

	void OnCollisionEnter(Collision collision) {
		//Debug.Log (collision.relativeVelocity.magnitude);
		if (collision.relativeVelocity.magnitude > minVel) {
			float volume = volumeCurve.Evaluate (collision.relativeVelocity.magnitude - minVel / maxVel - minVel);
			//Debug.Log (volume);
			SoundManager.PlaySound (hitSound, volume);
		}
	}
}
