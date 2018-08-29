using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartGameObject()
    {
        SceneManager.LoadScene("MainGameScene", LoadSceneMode.Single);
    }

    public void CloseGameObject()
    {
        Application.Quit();
    }

    public void CreditsGameObject()
    {
        SceneManager.LoadScene("Credits", LoadSceneMode.Single);
    }

    public void StartTutorialObject()
    {
        SceneManager.LoadScene("Tutorial Scene Copy", LoadSceneMode.Single);
    }
}
