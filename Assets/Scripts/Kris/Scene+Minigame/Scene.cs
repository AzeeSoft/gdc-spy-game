using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour {

	void Update () {
		if(Input.GetKeyDown("h"))
        {
            SceneManager.LoadScene(sceneName: "Minigame");
        }
	}
}
