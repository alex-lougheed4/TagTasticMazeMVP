using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	
	public void LoadGame()
	{
		SceneManager.LoadScene("Main");
		Debug.Log("Loaded to main scene");
	}

	public void QuitGame()
	{
		Application.Quit();
		Debug.Log("Quit!");
	}
}
