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
		lives -= 1;
		ShowLives ();
		if (lives <= 0) {
			SceneManager.LoadScene (0);
			Destroy (gameObject);
		} else {
			SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
		}
	}

	public void AdjustScore (int amount)
	{
		score += amount;
		ShowScore ();
	}
}
