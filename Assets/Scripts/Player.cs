using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MovingObject {

	public float restartLevelDelay = 1f;
	public int pointsPerFood = 10;
	public int pointsPerSoda = 20;
	public int wallDamage = 1;
	public Text foodText;

	private Animator animator;
	private int food;

	protected override void Start() {
		animator = GetComponent<Animator> ();
		food = GameManager.instance.playerFoodPoints;
		Debug.Log ("Player Start, food=" + food);
		foodText.text = "Food: " + food;
		base.Start ();
	}

	void OnDisable() {
		Debug.Log ("Player OnDisable");
		GameManager.instance.playerFoodPoints = food;
	}

	void Update() {
		if (!GameManager.instance.playersTurn)
			return;

		int horizontal = 0;
		int vertical = 0;
		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");
		if (horizontal != 0) {
			vertical = 0;
		}
		if (horizontal != 0 || vertical != 0) {
			AttemptMove (horizontal, vertical);
		}
	}

	void CheckIfGameOver() {
		if (food <= 0) {
			GameManager.instance.GameOver ();
		}
	}

	protected override void AttemptMove (int xDir, int yDir)
	{
		food--;
		foodText.text = "Food: " + food;
		base.AttemptMove (xDir, yDir);

		CheckIfGameOver ();
		GameManager.instance.playersTurn = false;
	}

	protected override void OnCantMove (Transform hitTransform)
	{
		Wall hitWall = hitTransform.GetComponent<Wall> ();
		if (hitWall != null) {
			hitWall.takeDamage (wallDamage);
			animator.SetTrigger ("playerChop");
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.CompareTag ("Exit")) {
			Invoke ("Restart", restartLevelDelay);
			gameObject.SetActive (false);
//			enabled = false;
		} else if (other.CompareTag ("Food")) {
			food += pointsPerFood;
			foodText.text = "+" + pointsPerFood + " Food: " + food;
			other.gameObject.SetActive (false);
		} else if (other.CompareTag ("Soda")) {
			food += pointsPerSoda;
			foodText.text = "+" + pointsPerSoda + " Food: " + food;
			other.gameObject.SetActive (false);
		}
	}

	void Restart() {
		SceneManager.LoadScene (0);
	}

	public void LoseFood(int loss) {
		animator.SetTrigger ("playerHit");
		food -= loss;
		foodText.text = "-" + loss + " Food: " + food;
		CheckIfGameOver ();
	}

}
