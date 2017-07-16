using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour {
	[SerializeField] private GameObject buttons;
	[SerializeField] private GameObject board;
	[SerializeField] private GameObject instructions;
	[SerializeField] private AudioClip selectSound;
	[SerializeField] private AudioClip closeSound;

	private enum MenuState {
		Main,
		Instructions,
		Leaderboard
	};

	MenuState menuState;

	private dreamloLeaderBoard leaderBoard;

	// Use this for initialization
	void Start () {
		GotoMain ();
		leaderBoard = dreamloLeaderBoard.GetSceneDreamloLeaderboard();
	}
	
	// Update is called once per frame
	void Update () {
		if (menuState == MenuState.Leaderboard) {
			GetLeaderboardString (leaderBoard);
		}
	}

	public void OpenInstructions() {
		SoundManager.PlaySound (selectSound);
		menuState = MenuState.Instructions;
		buttons.SetActive (false);
		board.SetActive (false);
		instructions.SetActive (true);
	}

	public void GotoMain() {
		SoundManager.PlaySound (closeSound);
		menuState = MenuState.Main;
		buttons.SetActive (true);
		board.SetActive (false);
		instructions.SetActive (false);
	}

	public void OpenLeaderboard() {
		SoundManager.PlaySound (selectSound);
		menuState = MenuState.Leaderboard;
		buttons.SetActive (false);
		board.SetActive (true);
		instructions.SetActive (false);

		leaderBoard.LoadScores ();
	}

	public void GotoMenu() {
		SceneManager.LoadScene (0);
	}

	public void GotoGame() {
		SoundManager.PlaySound (selectSound);
		SceneManager.LoadScene (1);
	}

	public void QuitGame() {
		SoundManager.PlaySound (selectSound);
		Application.Quit ();
	}

	public static void GetLeaderboardString(dreamloLeaderBoard lb) {
		List<dreamloLeaderBoard.Score> scores = lb.ToListHighToLow ().Take(10).ToList();
		if (scores != null && scores.Count > 0) {
			// parse scores
			string allScores = "";
			int index = 1;
			foreach (dreamloLeaderBoard.Score score in scores) {
				allScores += (
					(index > 9 ? "" : " ") + index.ToString() + ". " // position in list
					+ string.Format ("{0:F1}", Utils.ScoreToTime(score.score)) + "s | " // score
					+ score.playerName // name
					+ (score.playerName == PlayerPrefs.GetString ("Name") ? " <-" : "") // player indicator
					+"\n"
				);
				index += 1;
			}
			GameObject.Find ("Content").GetComponent<Text> ().text = allScores;
		}
	}
}
