using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void PlayGame ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        Debug.Log("PLay");
    }

    public void LoadGame ()
    {
        SceneManager.LoadScene("Credits");
        Debug.Log("Credits");
    }
    public void QuitGame ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
		
	
}
