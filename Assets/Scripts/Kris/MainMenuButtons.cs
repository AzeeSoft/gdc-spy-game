using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour {

    [Header("StartGame")]
    public GameObject StartGame;
    public GameObject ExitGame;



	// Use this for initialization
	void Start () {
        StartGame.GetComponent<Collider>();
        ExitGame.GetComponent<Collider>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseDown()
    {
        Debug.Log("Mouse down");
        if(true)
        {
            SceneManager.LoadScene("TutorialScene-F", LoadSceneMode.Single);
        }
    }
}
