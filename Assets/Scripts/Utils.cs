using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utils {
	private static int magicNumber = 10000000;
	public static int TimeToScore(float time) {
		return magicNumber - (int)(time * 1000);
	}

	public static float ScoreToTime(int score) {
		return (magicNumber - score) / 1000f;
	}
}
