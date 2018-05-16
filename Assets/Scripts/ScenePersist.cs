using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
	static int _startingIndex = -1;

	// Singleton
	static ScenePersist _instance;

	public static ScenePersist Instance {
		get { 
			if (_instance == null) {
				_instance = FindObjectOfType<ScenePersist> ();
				DontDestroyOnLoad (_instance.gameObject);
			}
			return _instance; 
		} 
	}

	void Awake ()
	{
		print ("ScenePersist Awake");
		if (_instance == null) {
			_instance = this;
		} else if (_instance != this) {
			// Most likely cause is a new level has been loaded
			int currentIndex = SceneManager.GetActiveScene ().buildIndex;

			print ("ScenePersist Awake - index: " + _startingIndex.ToString () + "/" + currentIndex.ToString ());
			if (currentIndex == _startingIndex) {
				// Keep original instance as the one singleton
				Destroy (gameObject);
				return;
			} else {
				// Destroy the previous instance and let this one take over
				Destroy (_instance.gameObject);
				_instance = this;
			}
		}
		_startingIndex = SceneManager.GetActiveScene ().buildIndex;
		DontDestroyOnLoad (gameObject);
	}

}
