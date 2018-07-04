using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenept2 : MonoBehaviour
{
  

    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            SceneManager.LoadScene(sceneName: "testK");
        }
    }
}
