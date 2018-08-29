using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveMaze : MonoBehaviour {

    public MazeSceneController onSceneSuccess;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Leaving the Maze");
        onSceneSuccess.MazeSceneResult(true);
    }
}
