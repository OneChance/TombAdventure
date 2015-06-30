using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{

	public float moveDistance = 0.5f;
	public float moveIntevalTime = 1;
	private float moveTimer;
	private StatusType currentStatus;
	private GameObject target;
	private Enemy enemy;

	public Enemy Enemy {
		get {
			return this.enemy;
		}
		set {
			enemy = value;
		}
	}

	private enum StatusType
	{  
		Patrol,  
		Trace
	}

	// Use this for initialization
	void Start ()
	{
		moveTimer = 0;
		currentStatus = StatusType.Patrol;

		//init enmey
		enemy = new Enemy (100, 5, 0, 0, "enemy_1", "enemy_1");
	}
	
	// Update is called once per frame
	void Update ()
	{
		moveTimer += Time.deltaTime;
		if (moveTimer > moveIntevalTime) {
			if (currentStatus == StatusType.Patrol) {
				Patrol ();
			} else if (currentStatus == StatusType.Trace) {
				Trace ();
			}
			moveTimer = 0;
		}
	}

	void Patrol ()
	{
		//patrol on a random route
		move (Random.Range (0, 4));
	}

	void Trace ()
	{
		Vector3 targetPos = target.transform.position;

		float xDelta = targetPos.x - transform.position.x;
		float yDelta = targetPos.y - transform.position.y;

		bool xMove = Mathf.Abs (xDelta) > GetComponent<SpriteRenderer> ().bounds.size.x / 2 + target.GetComponent<SpriteRenderer> ().bounds.size.x / 2;
		bool yMove = Mathf.Abs (yDelta) > GetComponent<SpriteRenderer> ().bounds.size.y / 2 + target.GetComponent<SpriteRenderer> ().bounds.size.y / 2;

		if ((Mathf.Abs (xDelta) <= Mathf.Abs (yDelta) && xMove) || !yMove) {
			if (xDelta > 0) {
				move (1);
			} else {
				move (0);
			}
		} else {
			if (yDelta > 0) {
				move (2);
			} else {
				move (3);
			}
		}
	}

	public void move (int direction)
	{
		if (direction == 0) {
			transform.Translate (Vector2.left * moveDistance);
		} else if (direction == 1) {
			transform.Translate (Vector2.right * moveDistance);
		} else if (direction == 2) {
			transform.Translate (Vector2.up * moveDistance);
		} else if (direction == 3) {
			transform.Translate (Vector2.down * moveDistance);
		}
	}

	void OnTriggerStay2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			target = other.gameObject;
			currentStatus = StatusType.Trace;
		}
	}

	void OnCollisionEnter2D (Collision2D coll)
	{
		if (coll.gameObject.tag == "GroundItem") {

		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			target = other.gameObject;
			currentStatus = StatusType.Trace;
		}
	}
}
