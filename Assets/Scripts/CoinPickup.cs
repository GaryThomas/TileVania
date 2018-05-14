using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
	[SerializeField] AudioClip coinPickupSFX;
	[SerializeField] int coinValue = 10;

	GameSession _gs;

	void Awake ()
	{
		_gs = GameSession.Instance;
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		_gs.AdjustScore (coinValue);
		AudioSource.PlayClipAtPoint (coinPickupSFX, Camera.main.transform.position);
		Destroy (gameObject);
	}
}
