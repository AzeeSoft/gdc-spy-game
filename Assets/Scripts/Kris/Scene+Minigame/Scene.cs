using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour {

	void Update () {
		if(Input.GetKeyDown("h"))
        {
            SceneManager.LoadScene("Maze2", LoadSceneMode.Additive);
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("Maze2"));
        }
    }
}
