using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{
	[SerializeField] int lives = 3;
	[SerializeField] TMP_Text livesText;

	// Singleton
	static GameSession _instance;

	public static GameSession Instance { get { return _instance; } }

	void Awake ()
	{
		if (_instance == null) {
			_instance = this;
			DontDestroyOnLoad (gameObject);
		} else if (_instance != this) {
			Destroy (gameObject);
		}
		ShowLives ();
	}

	void ShowLives ()
	{
		if (livesText) {
			livesText.text = lives.ToString ();
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
}
