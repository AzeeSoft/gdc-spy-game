using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenept2 : MonoBehaviour
{
    public Transform SavePos;


    void Update()
    {
        if (Input.GetKeyDown("h"))
        {
            SceneManager.LoadScene(sceneName: "testK");
        }
    }
}
