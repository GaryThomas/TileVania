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

	Rigidbody2D _rb;
	Animator _anim;

	void Awake ()
	{
		_rb = GetComponent<Rigidbody2D> ();
		_anim = GetComponent<Animator> ();
	}

	void Update ()
	{
		Move ();
	}

	void Move ()
	{
		float horiz = CrossPlatformInputManager.GetAxis ("Horizontal");
		float jump = CrossPlatformInputManager.GetButtonDown ("Jump") ? jumpSpeed : 0f;
		Vector2 velocity = new Vector2 (horiz * runSpeed, _rb.velocity.y + jump);
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
}
