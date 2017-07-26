using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MovingObject : MonoBehaviour {

	public float moveTime = 0.1f;
	public LayerMask blockingLayer;

	private BoxCollider2D boxCollider;
	private Rigidbody2D rb;
	private float inverseMoveTime;

	protected virtual void Start() {
		boxCollider = GetComponent<BoxCollider2D> ();
		rb = GetComponent<Rigidbody2D> ();
		inverseMoveTime = 1f / moveTime;
	}

	protected bool CanMove(int xDir, int yDir, out RaycastHit2D hit) {
		Vector2 start = transform.position;
		Vector2 end = start + new Vector2 (xDir, yDir);

		boxCollider.enabled = false;
		hit = Physics2D.Linecast (start, end, blockingLayer);
		boxCollider.enabled = true;

		if (hit.transform == null) {
			return true;
		} 
		return false;
	}

	protected IEnumerator SmoothMovement(Vector2 end) {
		float sqrRemainingDistance = ((Vector2)transform.position - end).sqrMagnitude;
		while(sqrRemainingDistance > float.Epsilon) {
			Vector2 newPosition = Vector2.MoveTowards (rb.position, end, inverseMoveTime * Time.deltaTime);
			rb.MovePosition (newPosition);
			sqrRemainingDistance = ((Vector2)transform.position - end).sqrMagnitude;
			yield return null;
		}
	}

	protected virtual void AttemptMove (int xDir, int yDir) {
		RaycastHit2D hit;
		bool canMove = CanMove (xDir, yDir, out hit);
		if (canMove) {
			Vector2 start = transform.position;
			Vector2 end = start + new Vector2 (xDir, yDir);

			StartCoroutine (SmoothMovement (end));
			return;
		}
		OnCantMove (hit.transform);
	}

	protected abstract void OnCantMove (Transform hitTransform);

}
