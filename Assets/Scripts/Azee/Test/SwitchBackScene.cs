using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchBackScene : MonoBehaviour
{
    public bool success = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	    if (Input.GetKeyDown(KeyCode.Backspace))
	    {
//	        Debug.Log("Switching to LightingTest");
	        FindObjectOfType<SceneSwitcher>().ShowLastSavedScene("MazeHackAttempt: " + (success?1:0));
	    }
    }
}
