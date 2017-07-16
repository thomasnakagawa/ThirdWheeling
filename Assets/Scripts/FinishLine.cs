using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using System.Linq;

public class FinishLine : MonoBehaviour {
	[SerializeField] private Text timerText;
	[SerializeField] private Text countDownText;
	[SerializeField] private Text endingText;
	[SerializeField] private InputField nameInput;
	[SerializeField] private GameObject board;
	[SerializeField] private GameObject buttons;
	[SerializeField] private vehicle player;
	[SerializeField] private AudioClip winSound;
	[SerializeField] private AudioClip selectSound;
	[SerializeField] private AudioClip countSound;
	[SerializeField] private AudioClip startSound;

	public enum GameState
	{
		Countdown,
		Play,
		Finish,
		Leaderboard,
	};

	private GameState gameState = GameState.Countdown;
	private float countdownTimer = 1f;
	private int countdownInt = 3;
	private float elapsedTime = 0f;

	private dreamloLeaderBoard leaderBoard;

	// Use this for initialization
	void Start () {
		endingText.gameObject.SetActive (false);
		timerText.gameObject.SetActive (false);
		buttons.SetActive (false);
		board.SetActive (false);
		leaderBoard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
		countDownText.text = "3";
		Assert.IsNotNull (leaderBoard);
		SoundManager.PlaySound (countSound);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Escape)) {
			GotoMenu ();
		}

		switch (gameState) {
		case GameState.Countdown:
			countdownTimer -= Time.deltaTime;
			if (countdownTimer < 0) {
				countdownInt -= 1;
				countdownTimer += 1f;
				countDownText.text = countdownInt.ToString ();
				if (countdownInt > 0) {
					SoundManager.PlaySound (countSound);
				}
			}

			if (countdownInt < 1) {
				SoundManager.PlaySound (startSound);
				gameState = GameState.Play;
				countDownText.gameObject.SetActive (false);
				timerText.gameObject.SetActive (true);
				player.Activate ();
			}
			break;
		case GameState.Play:
			elapsedTime += Time.deltaTime;
			timerText.text = string.Format ("{0:F1}", elapsedTime);
			break;
		case GameState.Finish:
			if (nameInput.isFocused && Input.GetKeyDown (KeyCode.Return)) {
				OnConfirmName ();
			}
			break;
		case GameState.Leaderboard:
			MenuManager.GetLeaderboardString (leaderBoard);
			break;
		default:
			throw new UnityException ("Bad");
		}
	}

	void OnTriggerEnter(Collider other) {
		if (gameState == GameState.Play) {
			gameState = GameState.Finish;
			timerText.gameObject.SetActive (false);
			endingText.gameObject.SetActive (true);
			endingText.text = "Congradulations!\nTime taken: " + string.Format ("{0:F2}", elapsedTime) + "s\nEnter your name into the leaderboard";
			nameInput.text = PlayerPrefs.GetString ("Name", "");
			nameInput.Select ();
			nameInput.selectionFocusPosition = nameInput.text.Length - 1;
			SoundManager.PlaySound (winSound);
		}
	}

	public void OnConfirmName() {
		SoundManager.PlaySound (selectSound);
		leaderBoard.AddScore (nameInput.text, Utils.TimeToScore(elapsedTime));
		PlayerPrefs.SetString ("Name", nameInput.text);
		endingText.gameObject.SetActive (false);
		board.SetActive (true);
		buttons.SetActive (true);
		gameState = GameState.Leaderboard;
		GameObject.Find ("Own").GetComponent<Text> ().text = "Your time: " + string.Format ("{0:F1}", elapsedTime) + "s";
	}
		
	public void GotoMenu() {
		SoundManager.PlaySound (selectSound);
		SceneManager.LoadScene (0);
	}

	public void GotoGame() {
		SoundManager.PlaySound (selectSound);
		SceneManager.LoadScene (1);
	}
}
