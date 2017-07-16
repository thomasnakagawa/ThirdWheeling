using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vehicle : MonoBehaviour {
	[SerializeField] private HingeJoint[] wheels;
	[SerializeField] private bool autoSpin;
	[SerializeField] private AudioClip engineSound;
	private bool ready = false;
	private bool menuSpin;

	// Use this for initialization
	void Start () {
		if (autoSpin) {
			ready = true;
			menuSpin = true;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (ready) {
			if (menuSpin && (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow))) {
				menuSpin = false;
			}
			if (Input.GetKeyDown (KeyCode.RightArrow) || Input.GetKeyDown (KeyCode.LeftArrow)) {
				SoundManager.PlaySound (engineSound);
			}

			if (Input.GetKey (KeyCode.RightArrow)) {
				foreach (HingeJoint joint in wheels) {
					joint.useMotor = true;
					JointMotor motor = joint.motor;
					motor.targetVelocity = 400f;
					motor.force = 100f;
					joint.motor = motor;
				}
			} else if (Input.GetKey (KeyCode.LeftArrow) || menuSpin) {
				foreach (HingeJoint joint in wheels) {
					joint.useMotor = true;
					JointMotor motor = joint.motor;
					motor.targetVelocity = -400f;
					motor.force = 100f;
					joint.motor = motor;
				}
			} else {
				foreach (HingeJoint joint in wheels) {
					joint.useMotor = false;
				}
			}
		}
	}

	public void Activate() {
		ready = true;
	}
}
