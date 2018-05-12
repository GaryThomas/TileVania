using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent (typeof(Rigidbody2D))]
[RequireComponent (typeof(Animator))]
public class Player : MonoBehaviour
{
	[SerializeField] float runSpeed = 10f;
	[SerializeField] float jumpSpeed = 28f;
	[SerializeField] float climbSpeed = 28f;
	[SerializeField] int health = 3;
	[SerializeField] Vector2 deathKnell = new Vector2 (15f, 20f);
	[SerializeField] int enemyDamage = 2;
	[SerializeField] int hazardsDamage = 1;
	[SerializeField] bool demoMode = false;
	[SerializeField] float resurrectionTime = 3f;

	Rigidbody2D _rb;
	Animator _anim;
	CapsuleCollider2D _bodyCollider;
	BoxCollider2D _feetCollider;
	LayerMask _groundMask;
	LayerMask _ladderMask;
	LayerMask _enemyMask;
	LayerMask _hazardsMask;
	Vector2 _origPosition;
	float _origGravity;
	bool _isAlive;
	int _health;

	void Awake ()
	{
		_rb = GetComponent<Rigidbody2D> ();
		_anim = GetComponent<Animator> ();
		_bodyCollider = GetComponent<CapsuleCollider2D> ();
		_feetCollider = GetComponent<BoxCollider2D> ();
		_groundMask = LayerMask.GetMask ("Ground");
		_ladderMask = LayerMask.GetMask ("Climbing");
		_enemyMask = LayerMask.GetMask ("Enemy");
		_hazardsMask = LayerMask.GetMask ("Hazards");
		_origGravity = _rb.gravityScale;
		_origPosition = transform.position;
		_isAlive = true;
	}

	void Update ()
	{
		if (demoMode) {
			DemoMode ();
		} else {
			if (_isAlive) {
				Run ();
				ClimbLadder ();
				Jump ();
			}
		}
	}

	void DemoMode ()
	{
		Vector2 velocity = new Vector2 (0.5f * runSpeed, _rb.velocity.y);
		_rb.velocity = velocity;
	}

	void Run ()
	{
		float horiz = CrossPlatformInputManager.GetAxis ("Horizontal");
		Vector2 velocity = new Vector2 (horiz * runSpeed, _rb.velocity.y);
		Vector2 scale = transform.localScale;
		_rb.velocity = velocity;
		if (Mathf.Abs (velocity.x) > Mathf.Epsilon) {
			_anim.SetBool ("Running", true);
			if (_rb.velocity.x < 0) {
				scale.x = Mathf.Abs (scale.x) * -1f;
			} else {
				scale.x = Mathf.Abs (scale.x) * 1f;
			}
			transform.localScale = scale;
		} else {
			_anim.SetBool ("Running", false);
		}
	}

	void ClimbLadder ()
	{
		if (_feetCollider.IsTouchingLayers (_ladderMask)) {
			float vert = CrossPlatformInputManager.GetAxis ("Vertical");
			Vector2 velocity = new Vector2 (_rb.velocity.x, vert * climbSpeed);
			_rb.velocity = velocity;
			_rb.gravityScale = 0f;
			_anim.SetBool ("Climbing", Mathf.Abs (_rb.velocity.y) > Mathf.Epsilon);
		} else {
			_rb.gravityScale = _origGravity;
			_anim.SetBool ("Climbing", false);
		}
	}

	void Jump ()
	{
		bool canJump = _feetCollider.IsTouchingLayers (_groundMask);
		float jump = canJump && CrossPlatformInputManager.GetButtonDown ("Jump") ? jumpSpeed : 0f;
		if (jump > 0f) {
			Vector2 velocity = _rb.velocity + new Vector2 (0f, jump);
			_rb.velocity = velocity;
		}
	}

	void Damage (int damage = 1)
	{
		_health -= damage;
		if (_health <= 0) {
			_isAlive = false;
			_anim.SetBool ("IsDead", true);
			_rb.velocity = deathKnell;
			StartCoroutine (ResurrectPlayer ());
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (_bodyCollider.IsTouchingLayers (_enemyMask)) {
			Damage (enemyDamage);
		} else if (_bodyCollider.IsTouchingLayers (_hazardsMask)) {
			Damage (hazardsDamage);
		}

	}

	void ResetPlayer ()
	{
		_isAlive = true;
		transform.position = _origPosition;
		_rb.gravityScale = _origGravity;
		_rb.velocity = Vector2.zero;
		_health = health;
		_anim.SetBool ("IsDead", false);
	}

	IEnumerator ResurrectPlayer ()
	{
		yield return new WaitForSeconds (resurrectionTime);
		ResetPlayer ();
	}
}
