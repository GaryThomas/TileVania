using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
	[SerializeField] int lives = 3;
	[SerializeField] TMP_Text livesText;
	[SerializeField] int score = 0;
	[SerializeField] TMP_Text scoreText;
	[SerializeField] float deathDisplayTime = 3f;
	[SerializeField] TMP_Text deathText;

	public static bool DemoScreen { get { return SceneManager.GetActiveScene ().buildIndex == 0; } }

	// Singleton
	static GameSession _instance;

	public static GameSession Instance {
		get { 
			if (_instance == null) {
				_instance = FindObjectOfType<GameSession> ();
				DontDestroyOnLoad (_instance.gameObject);
			}
			return _instance; 
		} 
	}

	void Awake ()
	{
		if (_instance == null) {
			_instance = this;
		} else if (_instance != this) {
			Destroy (gameObject);
			return;
		}
		DontDestroyOnLoad (gameObject);
	}

	void Start ()
	{
		ShowStatus ();
	}

	public void ShowStatus ()
	{
		ShowLives ();
		ShowScore ();
	}

	void ShowLives ()
	{
		if (livesText) {
			livesText.text = DemoScreen ? "" : lives.ToString ();
		}
	}

	void ShowScore ()
	{
		if (scoreText) {
			scoreText.text = DemoScreen ? "" : score.ToString ();
		}
	}

	public void PlayerDeath ()
	{
		if (!DemoScreen) {
			lives -= 1;
			ShowLives ();
			if (lives <= 0) {
				StartCoroutine (RestartGame ());
			} else {
				SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
			}
		}
	}

	IEnumerator RestartGame ()
	{
		deathText.enabled = true;
		yield return new WaitForSeconds (deathDisplayTime);
		SceneManager.LoadScene (0);
		Destroy (gameObject);
	}

	public void AdjustScore (int amount)
	{
		if (!DemoScreen) {
			score += amount;
			ShowScore ();
		}
	}
}
