using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePersist : MonoBehaviour
{
	int _startingIndex = -1;

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
			Destroy (gameObject);
			return;
		}
		_startingIndex = SceneManager.GetActiveScene ().buildIndex;
		DontDestroyOnLoad (gameObject);
	}

	void Start ()
	{
		print ("ScenePersist Start");
		_startingIndex = SceneManager.GetActiveScene ().buildIndex;
	}

	void OnEnable ()
	{
		print ("ScenePersist OnEnable");
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	//	void OnDisable ()
	//	{
	//		print ("ScenePersist OnDisable");
	//		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	//	}

	void OnLevelFinishedLoading (Scene scene, LoadSceneMode mode)
	{
		int currentIndex = SceneManager.GetActiveScene ().buildIndex;

		print ("ScenePersist OnLevelFinishedLoading - index: " + _startingIndex.ToString () + "/" + currentIndex.ToString ());
		if (_startingIndex != currentIndex) {
			SceneManager.sceneLoaded -= OnLevelFinishedLoading;
			Destroy (gameObject);
		}
	}
}
