using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MovingObject {
	public int playerDamage = 10;

	private Animator animator;
	private Transform target;
	private bool skipMove;

	protected override void Start ()
	{
		GameManager.instance.AddEnemyToList (this);
		animator = GetComponent <Animator> ();
		target = GameObject.FindWithTag ("Player").transform;
		base.Start ();
	}

	protected override void AttemptMove<T> (int xDir, int yDir)
	{
//		if (skipMove) {
//			skipMove = false;
//			return;
//		}

		base.AttemptMove<T> (xDir, yDir);
//		skipMove = true;
	}

	protected override void OnCantMove<T> (T component)
	{
		Debug.Log ("Enemy OnCantMove");
		Player hitPlayer = component as Player;
		hitPlayer.LoseFood (playerDamage);
		animator.SetTrigger ("enemyAttack");
	}

	public void MoveEnemy() {
		int xDir = 0;
		int yDir = 0;
		if (Mathf.Abs (target.position.x - transform.position.x) < float.Epsilon) {
			yDir = target.position.y > transform.position.y ? 1 : -1;
		} else {
			xDir = target.position.x > transform.position.x ? 1 : -1;
		}
		Debug.Log ("EnemyPosition: " + transform.position + ", PlayerPosition: " + target.position + ", movement:" + new Vector2(xDir, yDir));
		AttemptMove<Player> (xDir, yDir);
	}
}
