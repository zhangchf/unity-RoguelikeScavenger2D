using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
	public int playerDamage = 10;

	private Animator animator;
	private Transform target;
	private bool skipMove;
	private int moveDirection;

	protected override void Start ()
	{
		GameManager.instance.AddEnemyToList (this);
		animator = GetComponent <Animator> ();
		target = GameObject.FindWithTag ("Player").transform;
		base.Start ();
	}

	protected override void AttemptMove (int xDir, int yDir)
	{
//		if (skipMove) {
//			skipMove = false;
//			return;
//		}

		base.AttemptMove (xDir, yDir);
//		skipMove = true;
	}

	protected override void OnCantMove (Transform hitTransform)
	{
		Debug.Log ("Enemy OnCantMove");

		Player hitPlayer = hitTransform.GetComponent<Player> ();
		if (hitPlayer != null) {
			hitPlayer.LoseFood (playerDamage);
			animator.SetTrigger ("enemyAttack");
		} else {
			moveDirection++;
			if (moveDirection > 1) {
				return;
			} else {
				Move (moveDirection);
			}
		}
	}

	public void MoveEnemy() {
		moveDirection = 0;
		Move (moveDirection);
	}

	void Move(int moveDirection) {
		Vector2 movement = GetMovement (moveDirection);
		AttemptMove ((int)movement.x, (int)movement.y);
	}

	Vector2 GetMovement(int moveDirection) {
		int xDir = 0;
		int yDir = 0;
		if (moveDirection == 0) {
			if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) {
				yDir = target.position.y > transform.position.y ? 1 : -1;
			} else {
				xDir = target.position.x > transform.position.x ? 1 : -1;
			}
		} else if (moveDirection == 1) {
			if (Mathf.Abs (target.position.y - transform.position.y) < float.Epsilon) {
				xDir = target.position.x > transform.position.x ? 1 : -1;
			} else {
				yDir = target.position.y > transform.position.y ? 1 : -1;
			}
		}
		return new Vector2 (xDir, yDir);
	}
}
