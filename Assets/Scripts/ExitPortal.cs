using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{
	[SerializeField] float loadDelay = 0.5f;

	void OnTriggerEnter2D ()
	{
		StartCoroutine (ExitLevel ());
	}

	IEnumerator ExitLevel ()
	{
		yield return new WaitForSeconds (loadDelay);
		int currentSceneIndex = SceneManager.GetActiveScene ().buildIndex;
		SceneManager.LoadScene (currentSceneIndex + 1);  // FIXME
	}
}
