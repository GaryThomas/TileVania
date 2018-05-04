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

	Rigidbody2D _rb;
	Animator _anim;
	Collider2D _collider;
	LayerMask _groundMask;
	LayerMask _ladderMask;

	void Awake ()
	{
		_rb = GetComponent<Rigidbody2D> ();
		_anim = GetComponent<Animator> ();
		_collider = GetComponent<Collider2D> ();
		_groundMask = LayerMask.GetMask ("Ground");
		_ladderMask = LayerMask.GetMask ("Climbing");
	}

	void Update ()
	{
		Run ();
		ClimbLadder ();
		Jump ();
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
		if (_collider.IsTouchingLayers (_ladderMask)) {
			float vert = CrossPlatformInputManager.GetAxis ("Vertical");
			Vector2 velocity = _rb.velocity + new Vector2 (0f, vert * climbSpeed);
			print (velocity);
			_rb.velocity = velocity;
			_anim.SetBool ("Climbing", true);
		} else {
			_anim.SetBool ("Climbing", false);
		}
	}

	void Jump ()
	{
		bool canJump = _collider.IsTouchingLayers (_groundMask);
		float jump = canJump && CrossPlatformInputManager.GetButtonDown ("Jump") ? jumpSpeed : 0f;
		if (jump > 0f) {
			Vector2 velocity = _rb.velocity + new Vector2 (0f, jump);
			_rb.velocity = velocity;
		}
	}
}
