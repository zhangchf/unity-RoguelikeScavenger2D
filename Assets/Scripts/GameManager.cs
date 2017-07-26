using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static GameManager instance;

	public float levelStartDelay = 2f;
	public float turnDelay = 0.1f;

	public int playerFoodPoints = 100;
	public bool playersTurn = true;

	private Text levelText;
	private GameObject levelImage;
	private BoardManager boardScript;
	private int level = 0;
	private List<Enemy> enemies = new List<Enemy> ();
	private bool enemiesMoving;
	private bool doingSetup = true;

	void OnEnable() {
		Debug.Log ("GameManager OnEnable, playersTurn=" + playersTurn);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable() {
		Debug.Log ("GameManager OnDisable");
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
		Debug.Log ("GameManager OnSceneLoaded, before level=" + level + ", playerFood=" + playerFoodPoints);
		level++;
		InitGame ();
	}

	void Awake () {
		Debug.Log ("GameManager Awake" + ", playerFood=" + playerFoodPoints + ", levelStartDelay=" + levelStartDelay);
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}

		DontDestroyOnLoad (gameObject);
		boardScript = GetComponent<BoardManager> ();
	}


	void InitGame() {
		Debug.Log ("GameManager, InitGame, playersTurn=" + playersTurn);
		doingSetup = true;

		levelImage = GameObject.Find ("LevelImage");
		levelText = GameObject.Find ("LevelText").GetComponent<Text> ();
		levelText.text = "Day " + level;
		levelImage.SetActive (true);
		Invoke ("HideLevelImage", levelStartDelay);

		enemies.Clear ();
		boardScript.SetupScene (level);
	}

	void HideLevelImage() {
		levelImage.SetActive (false);
		doingSetup = false;
	}

	void Update() {
		if (playersTurn || enemiesMoving || doingSetup) {
			return;
		}
		StartCoroutine (MoveEnemies());
	}

	public void AddEnemyToList(Enemy script) {
		Debug.Log ("GameManager, AddEnemyToList");
		enemies.Add (script);
	}

	public void GameOver() {
		levelText.text = "After " + level + " days, you starved.";
		levelImage.SetActive (true);
		enabled = false;
	}

	IEnumerator MoveEnemies() {
		Debug.Log ("GameManager, MoveEnemies, playersTurn=" + playersTurn);
		enemiesMoving = true;

		yield return new WaitForSeconds (2f * turnDelay);
		if (enemies.Count == 0) {
			yield return new WaitForSeconds (turnDelay);
		}
		for (int i = 0; i < enemies.Count; i++) {
			enemies [i].MoveEnemy ();
			yield return new WaitForSeconds (enemies[i].moveTime);
		}

		playersTurn = true;
		enemiesMoving = false;
	}

}
