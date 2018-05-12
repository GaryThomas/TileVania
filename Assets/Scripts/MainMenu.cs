using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

	[SerializeField] int firstLevel = 1;

	public void StartGame ()
	{
		SceneManager.LoadScene (firstLevel);
	}
}
