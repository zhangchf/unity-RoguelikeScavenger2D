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

	public AudioClip[] moveClips;
	public AudioClip[] eatClips;
	public AudioClip[] drinkClips;
	public AudioClip[] chopClips;
	public AudioClip dieClip;

	private Animator animator;
	private int food;

	private Vector2 touchOrigin = -Vector2.one;

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

		#if UNITY_STANDALONE
		horizontal = (int)Input.GetAxisRaw ("Horizontal");
		vertical = (int)Input.GetAxisRaw ("Vertical");
		if (horizontal != 0) {
			vertical = 0;
		}
		#else
		if (Input.touchCount > 0) {
			Touch touch = Input.touches[0];
			if (touch.phase == TouchPhase.Began) {
				touchOrigin = touch.position;
			} else if (touch.phase == TouchPhase.Ended && touchOrigin.x >= 0) {
				Vector2 touchEnd = touch.position;
				float x = touchEnd.x - touchOrigin.x;
				float y = touchEnd.y - touchOrigin.y;
				touchOrigin = -Vector2.one;
				if (Mathf.Abs (x) > Mathf.Abs (y)) {
					horizontal = x > 0 ? 1 : -1;
				} else {
					vertical = y > 0 ? 1 : -1;
				}
			}
		}
		#endif

		if (horizontal != 0 || vertical != 0) {
			AttemptMove (horizontal, vertical);
		}
	}

	void CheckIfGameOver() {
		if (food <= 0) {
			SoundManager.instance.PlaySingleSfx (dieClip);
			SoundManager.instance.musicSource.Stop ();
			GameManager.instance.GameOver ();
		}
	}

	protected override void AttemptMove (int xDir, int yDir)
	{
		food--;
		foodText.text = "Food: " + food;
		base.AttemptMove (xDir, yDir);

		RaycastHit2D hit;
		if (CanMove (xDir, yDir, out hit)) {
			SoundManager.instance.RandomizeSfx (moveClips);
		}

		CheckIfGameOver ();
		GameManager.instance.playersTurn = false;
	}

	protected override void OnCantMove (Transform hitTransform)
	{
		Wall hitWall = hitTransform.GetComponent<Wall> ();
		if (hitWall != null) {
			hitWall.takeDamage (wallDamage);
			animator.SetTrigger ("playerChop");
			SoundManager.instance.RandomizeSfx (chopClips);
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
			SoundManager.instance.RandomizeSfx (eatClips);
		} else if (other.CompareTag ("Soda")) {
			food += pointsPerSoda;
			foodText.text = "+" + pointsPerSoda + " Food: " + food;
			other.gameObject.SetActive (false);
			SoundManager.instance.RandomizeSfx (drinkClips);
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
