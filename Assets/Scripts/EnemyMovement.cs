using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
	[SerializeField] float moveSpeed = 1f;

	Rigidbody2D _rb;
	LayerMask _groundLayerMask;
	BoxCollider2D _feetCollider;

	void Awake ()
	{
		_rb = GetComponent<Rigidbody2D> ();
		_feetCollider = GetComponent<BoxCollider2D> ();
		_groundLayerMask = LayerMask.GetMask ("Ground");
	}

	void Update ()
	{
		_rb.velocity = new Vector2 (Mathf.Sign (transform.localScale.x) * moveSpeed, 0f);
	}

	void OnTriggerExit2D (Collider2D other)
	{
		print ("Trigger leaving " + other.name);
		if (!_feetCollider.IsTouchingLayers (_groundLayerMask)) {
			transform.localScale = new Vector2 (-transform.localScale.x, transform.localScale.y);
		}
	}
}
